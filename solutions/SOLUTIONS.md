# Library Management System - Complete Solutions

This document contains the complete solutions for all parts of the interview question. Use this for evaluation and comparison purposes.

## Part 1: Database Schema Analysis

### Schema Review

The provided schema includes:
- **authors**: Basic author information
- **books**: Book metadata with ISBN
- **book_authors**: Many-to-many relationship between books and authors
- **book_copies**: Multiple physical copies of each book
- **users**: Library member information
- **checkouts**: Loan tracking with dates and fees

**Key Design Decisions:**
1. Separated books from book_copies to handle multiple physical copies
2. Many-to-many relationship for books and authors (e.g., Good Omens)
3. Checkout tracks specific book copy, not just book
4. Indexes on frequently queried columns for performance

### SQL Solutions

See `sql-queries.sql` for complete query solutions:

1. **Current Checkouts Query**:
   - Joins checkouts, users, book_copies, and books
   - Filters for `is_returned = false`
   - Shows overdue status

2. **Overdue Users Query**:
   - Finds users with books past due date
   - Groups to count overdue items per user
   - Uses date comparison with current date

3. **Popular Authors Query**:
   - Aggregates checkout counts by author
   - Handles co-authored books correctly
   - Includes both total checkouts and unique books

## Part 2: C# Implementation

### Key Implementation Points

**Checkout Business Logic:**
```csharp
public CheckoutResult CheckoutBook(string isbn, int copyNumber, int userCardNumber)
{
    // 1. Validate user can checkout (no overdue books)
    // 2. Find and verify book copy exists
    // 3. Check book copy availability
    // 4. Create checkout record with 14-day due date
    // 5. Return success/failure result
}
```

**Return with Late Fee Calculation:**
```csharp
public ReturnResult ReturnBook(string isbn, int copyNumber)
{
    // 1. Find active checkout for book copy
    // 2. Calculate days overdue (max 0)
    // 3. Apply $0.50 per day late fee
    // 4. Update checkout record
    // 5. Return fee information
}
```

**Data Access Patterns:**
- Use Dapper for efficient SQL mapping
- Proper connection management with `using` statements
- Error handling with try-catch blocks
- Return appropriate result objects

### Critical Business Rules

1. **User Checkout Validation**: Users with any overdue books cannot checkout new items
2. **Copy Availability**: Each book copy can only be checked out once
3. **Due Date**: 14 days from checkout date
4. **Late Fees**: $0.50 per day overdue, calculated on return

## Part 3: Integration Demo

### Expected Workflow

The demonstration should show:

1. **Initial State**: Alice has overdue "1984", Bob has "Great Gatsby"
2. **Failed Checkout**: Alice tries to get "Good Omens" → blocked by overdue book
3. **Return Processing**: Alice returns "1984" → calculates late fee
4. **Successful Checkout**: Alice can now checkout "Good Omens"
5. **Final State**: Display updated checkout status

### Sample Output

```
Library Management System - Interview Question
============================================

📚 Current Library Status:
   Alice Johnson:
     - 1984 (Copy 1) - Due: 2025-09-08 ⚠️  OVERDUE
   Bob Smith:
     - The Great Gatsby (Copy 1) - Due: 2025-09-15 ✅ On time

🔍 Attempting to check out 'Good Omens' to Alice...
❌ User has overdue books and cannot check out new items

📖 Alice returning '1984'...
✅ Book returned successfully!
   Late fee: $0.50 (1 days overdue)

🔍 Attempting to check out 'Good Omens' to Alice again...
✅ Successfully checked out 'Good Omens' copy 1
   Due Date: 2025-09-22

📋 Final Library Status:
   Alice Johnson:
     - Good Omens (Copy 1) - Due: 2025-09-22 ✅ On time
   Bob Smith:
     - The Great Gatsby (Copy 1) - Due: 2025-09-15 ✅ On time
```

## Evaluation Rubric

### Excellent (90-100%)
- ✅ All SQL queries correct and efficient
- ✅ Complete C# implementation with proper error handling
- ✅ All business rules correctly implemented
- ✅ Clean, readable code with good practices
- ✅ Comprehensive demonstration with clear output

### Good (70-89%)
- ✅ Most SQL queries correct
- ✅ Core C# methods implemented correctly
- ⚠️ Minor issues with error handling or edge cases
- ✅ Basic demonstration works
- ⚠️ Some code quality issues

### Satisfactory (50-69%)
- ⚠️ SQL queries mostly work but may have efficiency issues
- ⚠️ C# implementation has gaps or bugs
- ❌ Some business rules not correctly implemented
- ⚠️ Demonstration incomplete or has errors

### Needs Improvement (<50%)
- ❌ Major SQL query errors
- ❌ C# code doesn't compile or has significant bugs
- ❌ Business logic fundamentally flawed
- ❌ Cannot demonstrate working solution

## Common Pitfalls

### Database Issues
1. **Date Comparisons**: Using wrong date format or timezone
2. **JOIN Logic**: Incorrect joins leading to wrong results
3. **NULL Handling**: Not accounting for optional fields

### C# Implementation Issues
1. **Connection Management**: Not disposing connections properly
2. **SQL Injection**: Not using parameterized queries
3. **Error Handling**: Not handling database exceptions
4. **Business Logic**: Missing validation steps

### Integration Problems
1. **Hardcoded Values**: Not using proper test data
2. **Output Format**: Poor presentation of results
3. **Edge Cases**: Not testing overdue scenarios

## Bonus Features

Advanced candidates might implement:
- Transaction handling for data consistency
- Input validation and sanitization
- Logging and monitoring
- Configuration management
- Additional business features (holds, renewals, etc.)

## Files Reference

- `sql-queries.sql` - Complete SQL solutions
- `LibraryManager-solution.cs` - Full C# implementation
- `Program-solution.cs` - Complete demonstration
- Main project files in `/src` - Scaffold for candidates