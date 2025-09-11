using LibraryManagement.Models;
using Dapper;
using Npgsql;

namespace LibraryManagement.Solutions;

/// <summary>
/// Complete implementation of LibraryManager - this is the expected solution
/// Candidates should implement similar logic in the main LibraryManager.cs file
/// </summary>
public class LibraryManagerSolution
{
    private readonly DatabaseConnection _dbConnection;

    public LibraryManagerSolution(DatabaseConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public CheckoutResult CheckoutBook(string isbn, int copyNumber, int userCardNumber)
    {
        try
        {
            using var connection = GetConnection();
            
            // 1. Check if user exists and can checkout
            if (!CanUserCheckout(userCardNumber))
            {
                return new CheckoutResult 
                { 
                    Success = false, 
                    Message = "User has overdue books and cannot check out new items" 
                };
            }

            // 2. Find the specific book copy
            var bookCopy = connection.QueryFirstOrDefault<dynamic>(@"
                SELECT bc.id, bc.book_id, bc.copy_number, b.title, b.isbn
                FROM book_copies bc
                JOIN books b ON bc.book_id = b.id
                WHERE b.isbn = @isbn AND bc.copy_number = @copyNumber",
                new { isbn, copyNumber });

            if (bookCopy == null)
            {
                return new CheckoutResult 
                { 
                    Success = false, 
                    Message = "Book copy not found" 
                };
            }

            // 3. Check if book copy is available
            var isAvailable = connection.QueryFirstOrDefault<bool>(@"
                SELECT CASE WHEN COUNT(*) = 0 THEN true ELSE false END
                FROM checkouts 
                WHERE book_copy_id = @bookCopyId AND is_returned = false",
                new { bookCopyId = bookCopy.id });

            if (!isAvailable)
            {
                return new CheckoutResult 
                { 
                    Success = false, 
                    Message = "Book copy is already checked out" 
                };
            }

            // 4. Get user ID
            var userId = connection.QueryFirstOrDefault<int>(@"
                SELECT id FROM users WHERE library_card_number = @userCardNumber",
                new { userCardNumber });

            if (userId == 0)
            {
                return new CheckoutResult 
                { 
                    Success = false, 
                    Message = "User not found" 
                };
            }

            // 5. Create checkout record
            var checkoutDate = DateTime.Today;
            var dueDate = checkoutDate.AddDays(14);

            connection.Execute(@"
                INSERT INTO checkouts (book_copy_id, user_id, checkout_date, due_date, is_returned)
                VALUES (@bookCopyId, @userId, @checkoutDate, @dueDate, false)",
                new { 
                    bookCopyId = bookCopy.id, 
                    userId, 
                    checkoutDate, 
                    dueDate 
                });

            return new CheckoutResult 
            { 
                Success = true, 
                Message = $"Successfully checked out '{bookCopy.title}' copy {copyNumber}",
                DueDate = dueDate
            };
        }
        catch (Exception ex)
        {
            return new CheckoutResult 
            { 
                Success = false, 
                Message = $"Error during checkout: {ex.Message}" 
            };
        }
    }

    public ReturnResult ReturnBook(string isbn, int copyNumber)
    {
        try
        {
            using var connection = GetConnection();
            
            // Find the active checkout for this book copy
            var checkout = connection.QueryFirstOrDefault<dynamic>(@"
                SELECT c.id, c.due_date, c.checkout_date, u.name as user_name, b.title
                FROM checkouts c
                JOIN book_copies bc ON c.book_copy_id = bc.id
                JOIN books b ON bc.book_id = b.id
                JOIN users u ON c.user_id = u.id
                WHERE b.isbn = @isbn 
                    AND bc.copy_number = @copyNumber 
                    AND c.is_returned = false",
                new { isbn, copyNumber });

            if (checkout == null)
            {
                return new ReturnResult 
                { 
                    Success = false, 
                    LateFee = 0, 
                    DaysOverdue = 0 
                };
            }

            // Calculate late fee
            var returnDate = DateTime.Today;
            var dueDate = (DateTime)checkout.due_date;
            var daysOverdue = Math.Max(0, (returnDate - dueDate).Days);
            var lateFee = daysOverdue * 0.50m;

            // Update checkout record
            connection.Execute(@"
                UPDATE checkouts 
                SET return_date = @returnDate, 
                    late_fee = @lateFee, 
                    is_returned = true
                WHERE id = @checkoutId",
                new { 
                    returnDate, 
                    lateFee, 
                    checkoutId = checkout.id 
                });

            return new ReturnResult 
            { 
                Success = true, 
                LateFee = lateFee, 
                DaysOverdue = daysOverdue 
            };
        }
        catch (Exception ex)
        {
            return new ReturnResult 
            { 
                Success = false, 
                LateFee = 0, 
                DaysOverdue = 0 
            };
        }
    }

    public List<CheckedOutBook> GetUserCheckouts(int userCardNumber)
    {
        try
        {
            using var connection = GetConnection();
            
            var checkouts = connection.Query<CheckedOutBook>(@"
                SELECT 
                    b.isbn as Isbn,
                    b.title as Title,
                    bc.copy_number as CopyNumber,
                    c.checkout_date as CheckoutDate,
                    c.due_date as DueDate
                FROM checkouts c
                JOIN users u ON c.user_id = u.id
                JOIN book_copies bc ON c.book_copy_id = bc.id
                JOIN books b ON bc.book_id = b.id
                WHERE u.library_card_number = @userCardNumber 
                    AND c.is_returned = false
                ORDER BY c.due_date",
                new { userCardNumber });

            return checkouts.ToList();
        }
        catch
        {
            return new List<CheckedOutBook>();
        }
    }

    public List<int> GetAvailableCopies(string isbn)
    {
        try
        {
            using var connection = GetConnection();
            
            var availableCopies = connection.Query<int>(@"
                SELECT bc.copy_number
                FROM book_copies bc
                JOIN books b ON bc.book_id = b.id
                LEFT JOIN checkouts c ON bc.id = c.book_copy_id AND c.is_returned = false
                WHERE b.isbn = @isbn AND c.id IS NULL
                ORDER BY bc.copy_number",
                new { isbn });

            return availableCopies.ToList();
        }
        catch
        {
            return new List<int>();
        }
    }

    public bool CanUserCheckout(int userCardNumber)
    {
        try
        {
            using var connection = GetConnection();
            
            var overdueCount = connection.QueryFirstOrDefault<int>(@"
                SELECT COUNT(*)
                FROM checkouts c
                JOIN users u ON c.user_id = u.id
                WHERE u.library_card_number = @userCardNumber 
                    AND c.is_returned = false
                    AND c.due_date < CURRENT_DATE",
                new { userCardNumber });

            return overdueCount == 0;
        }
        catch
        {
            return false;
        }
    }

    protected NpgsqlConnection GetConnection()
    {
        return _dbConnection.GetConnection();
    }
}