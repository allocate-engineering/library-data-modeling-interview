using LibraryManagement;

/// <summary>
/// Complete solution for Program.cs demonstration
/// Shows the expected implementation for Part 3 of the interview
/// </summary>
class ProgramSolution
{
    static void Main(string[] args)
    {
        Console.WriteLine("Library Management System - Interview Question");
        Console.WriteLine("============================================");
        Console.WriteLine();
        
        var dbConnection = new DatabaseConnection();
        var libraryManager = new LibraryManager(dbConnection);

        try 
        {
            // 1. Display current status
            Console.WriteLine("📚 Current Library Status:");
            DisplayCurrentCheckouts(libraryManager);
            Console.WriteLine();

            // 2. Attempt to check out a book to Alice (should fail - she has overdue books)
            Console.WriteLine("🔍 Attempting to check out 'Good Omens' to Alice...");
            var checkoutResult = libraryManager.CheckoutBook("9780060853983", 1, 12345);
            
            if (checkoutResult.Success)
            {
                Console.WriteLine($"✅ {checkoutResult.Message}");
                if (checkoutResult.DueDate.HasValue)
                    Console.WriteLine($"   Due Date: {checkoutResult.DueDate.Value:yyyy-MM-dd}");
            }
            else
            {
                Console.WriteLine($"❌ {checkoutResult.Message}");
            }
            Console.WriteLine();

            // 3. Alice returns her overdue book
            Console.WriteLine("📖 Alice returning '1984'...");
            var returnResult = libraryManager.ReturnBook("9780451524935", 1);
            
            if (returnResult.Success)
            {
                Console.WriteLine($"✅ Book returned successfully!");
                if (returnResult.DaysOverdue > 0)
                {
                    Console.WriteLine($"   Late fee: ${returnResult.LateFee:F2} ({returnResult.DaysOverdue} days overdue)");
                }
                else
                {
                    Console.WriteLine("   Returned on time - no late fee");
                }
            }
            else
            {
                Console.WriteLine("❌ Failed to return book");
            }
            Console.WriteLine();

            // 4. Successfully check out a new book to Alice
            Console.WriteLine("🔍 Attempting to check out 'Good Omens' to Alice again...");
            var newCheckoutResult = libraryManager.CheckoutBook("9780060853983", 1, 12345);
            
            if (newCheckoutResult.Success)
            {
                Console.WriteLine($"✅ {newCheckoutResult.Message}");
                if (newCheckoutResult.DueDate.HasValue)
                    Console.WriteLine($"   Due Date: {newCheckoutResult.DueDate.Value:yyyy-MM-dd}");
            }
            else
            {
                Console.WriteLine($"❌ {newCheckoutResult.Message}");
            }
            Console.WriteLine();

            // 5. Display final status
            Console.WriteLine("📋 Final Library Status:");
            DisplayCurrentCheckouts(libraryManager);
            
            // Bonus: Show available copies
            Console.WriteLine();
            Console.WriteLine("📦 Available Book Copies:");
            DisplayAvailableCopies(libraryManager);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"💥 Error: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
        }

        Console.WriteLine();
        Console.WriteLine("Interview demonstration complete!");
    }

    private static void DisplayCurrentCheckouts(LibraryManager libraryManager)
    {
        // Display checkouts for all known users
        var users = new[] { 12345, 67890, 11111 }; // Alice, Bob, Charlie
        var userNames = new[] { "Alice Johnson", "Bob Smith", "Charlie Brown" };

        for (int i = 0; i < users.Length; i++)
        {
            var checkouts = libraryManager.GetUserCheckouts(users[i]);
            if (checkouts.Any())
            {
                Console.WriteLine($"   {userNames[i]}:");
                foreach (var checkout in checkouts)
                {
                    var status = checkout.IsOverdue ? "⚠️  OVERDUE" : "✅ On time";
                    Console.WriteLine($"     - {checkout.Title} (Copy {checkout.CopyNumber}) - Due: {checkout.DueDate:yyyy-MM-dd} {status}");
                }
            }
            else
            {
                Console.WriteLine($"   {userNames[i]}: No active checkouts");
            }
        }
    }

    private static void DisplayAvailableCopies(LibraryManager libraryManager)
    {
        var books = new[] 
        { 
            ("9780743273565", "The Great Gatsby"),
            ("9780451524935", "1984"),
            ("9780060853983", "Good Omens")
        };

        foreach (var (isbn, title) in books)
        {
            var availableCopies = libraryManager.GetAvailableCopies(isbn);
            if (availableCopies.Any())
            {
                Console.WriteLine($"   {title}: Copies {string.Join(", ", availableCopies)} available");
            }
            else
            {
                Console.WriteLine($"   {title}: All copies checked out");
            }
        }
    }
}