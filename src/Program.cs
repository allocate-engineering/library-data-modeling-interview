using System.Runtime.InteropServices;
using LibraryManagement;
using LibraryManagement.Data;
using LibraryManagement.Models;

// This is the main entry point for the candidate to demonstrate their solution
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Library Management System - Interview Question");
        Console.WriteLine("==============================================");

        var dbConnection = new DatabaseConnection();
        var libraryManager = new LibraryManager(dbConnection);

        // TODO: Candidates should implement the demonstration logic here
        // The requirements are:
        // 1. Load the sample data (already done via Liquibase)
        // 2. Attempt to check out a book to Alice (who has an overdue book)
        // 3. Alice returning her overdue book and paying the fee
        // 4. Successfully checking out a new book to Alice
        // 5. Display the final status

        Console.WriteLine("Candidate should implement demonstration logic in Main method");

        // Example of what the you might implement:
        // try 
        // {
        //     // Try to check out a book to Alice (should fail - she has overdue books)
        //     var checkoutResult = libraryManager.CheckoutBook("9780060853983", 1, 12345);
        //     
        //     // Alice returns her overdue book
        //     var returnResult = libraryManager.ReturnBook("9780451524935", 1);
        //     
        //     // Now Alice can check out a new book
        //     var newCheckoutResult = libraryManager.CheckoutBook("9780060853983", 1, 12345);
        //     
        //     // Display results
        // }
        // catch (Exception ex)
        // {
        //     Console.WriteLine($"Error: {ex.Message}");
        // }
    }
}