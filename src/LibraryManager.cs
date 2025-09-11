using LibraryManagement.Data;
using LibraryManagement.Models;

namespace LibraryManagement;

/// <summary>
/// Core business logic for library management operations
/// 
/// TODO: Implement these methods based on your domain model design.
/// Use the Repository<T> class for basic CRUD operations.
/// 
/// Example usage:
/// var userRepo = new Repository<User>(_dbConnection);
/// var user = await userRepo.GetByIdAsync(userId);
/// </summary>
public class LibraryManager
{
    private readonly DatabaseConnection _dbConnection;

    public LibraryManager(DatabaseConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    /// <summary>
    /// Check out a book to a user
    /// </summary>
    /// <param name="isbn">ISBN of the book to checkout</param>
    /// <param name="copyNumber">Copy number of the specific book</param>
    /// <param name="userCardNumber">User's library card number</param>
    /// <returns>Result indicating success or failure with details</returns>
    public CheckoutResult CheckoutBook(string isbn, int copyNumber, int userCardNumber)
    {
        // TODO: Implement this method
        // Business rules to enforce:
        // 1. Verify user exists and can check out books (no overdue items)
        // 2. Verify book copy exists and is available
        // 3. Create checkout record with due date (14 days from today)
        // 4. Return appropriate result
        //
        // Example implementation pattern:
        // var userRepo = new Repository<User>(_dbConnection);
        // var user = await userRepo.QueryFirstOrDefaultAsync(
        //     "SELECT * FROM users WHERE library_card_number = @cardNumber", 
        //     new { cardNumber = userCardNumber });
        
        throw new NotImplementedException("Candidate should implement this method");
    }

    /// <summary>
    /// Return a book and calculate any late fees
    /// </summary>
    /// <param name="isbn">ISBN of the book being returned</param>
    /// <param name="copyNumber">Copy number of the specific book</param>
    /// <returns>Result with late fee information</returns>
    public ReturnResult ReturnBook(string isbn, int copyNumber)
    {
        // TODO: Implement this method
        // Business rules:
        // 1. Find the active checkout for this book copy
        // 2. Calculate late fee ($0.50 per day overdue)
        // 3. Update checkout record with return date and late fee
        // 4. Return result with fee information
        
        throw new NotImplementedException("Candidate should implement this method");
    }

    /// <summary>
    /// Get all books currently checked out by a user
    /// </summary>
    /// <param name="userCardNumber">User's library card number</param>
    /// <returns>List of books currently checked out by the user</returns>
    public List<CheckedOutBook> GetUserCheckouts(int userCardNumber)
    {
        // TODO: Implement this method
        // Should return all active checkouts for the specified user
        // You'll need to design the CheckedOutBook result class
        
        throw new NotImplementedException("Candidate should implement this method");
    }

    /// <summary>
    /// Find available copies of a book
    /// </summary>
    /// <param name="isbn">ISBN of the book</param>
    /// <returns>List of copy numbers that are available for checkout</returns>
    public List<int> GetAvailableCopies(string isbn)
    {
        // TODO: Implement this method
        // Should return copy numbers that are not currently checked out
        
        throw new NotImplementedException("Candidate should implement this method");
    }

    /// <summary>
    /// Check if a user can check out books (no overdue items)
    /// </summary>
    /// <param name="userCardNumber">User's library card number</param>
    /// <returns>True if user can check out books, false if they have overdue items</returns>
    public bool CanUserCheckout(int userCardNumber)
    {
        // TODO: Implement this method
        // Should return false if user has any overdue books
        
        throw new NotImplementedException("Candidate should implement this method");
    }

    // Helper: Create a repository for any of your domain models
    protected Repository<T> GetRepository<T>() where T : class
    {
        return new Repository<T>(_dbConnection);
    }
}