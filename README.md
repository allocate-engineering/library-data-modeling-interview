# Library Management System - Domain Modeling Interview
This interview question is designed to test your **domain modeling skills** at both the database and object-oriented programming layers.

## Overview

You're tasked with designing and implementing a basic library management system from scratch. To do this you'll need to:

1. **Analyze business requirements** and identify key entities and relationships
2. **Design a database schema** that enforces business rules and supports efficient queries  
3. **Model the domain in C#** that accurately represent the business domain
4. **Implement business logic** that handles real-world constraints and edge cases

## Getting Started

### Prerequisites
- Visual Studio Code with Dev Containers extension
- Docker Desktop

### Setup Instructions

1. **Open in Dev Container**
   - Open this project in VS Code
   - When prompted, select "Reopen in Container" 
   - Wait for the devcontainer to build and start
   - Run `docker ps`, you should now see 3 containers running (one for the container, one for postgres, and one for pgAdmin)

2. **Confirm you can access the database through pgAdmin**
   
   **pgAdmin (Web Interface)**
   - **pgAdmin**: [http://localhost:5050](http://localhost:5050)
     - Username: `admin@example.com`
     - Password: `librarypass`
     - The connection to your local DB should be auto-registered and ready for you when you login, but you'll need the database's password to connect which is `bookworm123`.

3. **Verify Setup (at the root of the solution)**
   ```bash   
   # Build C# project
   dotnet build
   ```

4. **After Designing Your Schema** (Part 1 of interview)
   ```bash
   # Run Liquibase to create your schema and load sample data
   cd db
   liquibase update
   ```

## Business Requirements

You are building a library management system with these **core requirements**:

### Functional Requirements

**Books & Authors**
- Track books with ISBN, title, publication year
- Books can have multiple authors (e.g., "Good Omens" by Terry Pratchett & Neil Gaiman)
- Authors have names, birth years, and nationalities
- Libraries stock multiple physical copies of popular books
- Each physical copy has a condition (excellent, good, fair, poor)

**Library Users**
- Users have unique library card numbers, names, emails, and "membership sign-up" dates
- Track user status (active/inactive)
- Users can have multiple books checked out simultaneously

**Checkout System**
- Users check out specific physical copies, not just "books"
- Checkout period is 14 days from checkout date
- Track checkout date, due date, and return date
- Calculate late fees at $0.05 per day overdue (this amount may change in the future)
- Keep historical record of all checkouts (past and current)

**Business Rules**
- A physical copy can only be checked out by one user at a time
- Users with overdue books cannot check out additional books
- Books must be returned before checking out new ones if user has overdue items
- System must prevent double-booking of the same physical copy

### Non-Functional Requirements
- Support efficient queries for common operations (user's books, overdue books, popular authors)
- Maintain data integrity and referential consistency
- Support future extensions (holds, renewals, reservations)

## Part 1: Domain Analysis & Design

### Your Tasks

**Step 1: Entity Identification**
- Identify the core domain entities and their attributes
- Map out the relationships between entities (1:1, 1:many, many:many)
- Consider the distinction between logical concepts (books) vs physical items (copies)

**Step 2: Database Schema Design**  
- Design tables with appropriate columns, data types, and constraints
- Define primary keys, foreign keys, and unique constraints
- Add indexes for common query patterns
- Implement the schema in `db/library-schema.sql`

**Step 3: Object Model Design**
- Design C# classes that represent your domain entities
- Define properties, relationships, and navigation patterns
- Consider how your objects will map to database tables
- Implement models in `src/Models/` directory

### Sample Data Requirements

After completing your schema, populate it with this test data.  This can be done in `db/sample-data.sql` as a second liquibase changeset:

**Books:**
- "The Great Gatsby" (ISBN: 9780743273565, 1925) by F. Scott Fitzgerald (American, 1896)
- "1984" (ISBN: 9780451524935, 1949) by George Orwell (British, 1903)  
- "Good Omens" (ISBN: 9780060853983, 1990) by Terry Pratchett (British, 1948) & Neil Gaiman (British, 1960)

**Users:**
- Alice Johnson (Card: 12345, joined: 2023-01-15)
- Bob Smith (Card: 67890, joined: 2023-03-22)
- Charlie Brown (Card: 11111, joined: 2023-06-10)

**Current State (as of "today"):**
- Alice has "1984" copy 1 checked out (overdue since 3 days ago)
- Bob has "The Great Gatsby" copy 1 checked out (due five days from today)
- All books have 2 physical copies available

## Part 2: Implementation

### Core Business Logic

Implement these methods in `src/LibraryManager.cs`:

```csharp
public class LibraryManager
{
    // Check out a book to a user - handle all business rules
    public CheckoutResult CheckoutBook(string isbn, int copyNumber, int userCardNumber);
    
    // Return a book and calculate late fees
    public ReturnResult ReturnBook(string isbn, int copyNumber);
    
    // Get all books currently checked out by a user  
    public List<CheckedOutBook> GetUserCheckouts(int userCardNumber);
    
    // Find available copies of a book
    public List<int> GetAvailableCopies(string isbn);
    
    // Check if user can checkout books (no overdue items)
    public bool CanUserCheckout(int userCardNumber);
}
```

### Key Business Logic to Implement

1. **Checkout Validation**
   - Verify user exists and is active
   - Check user has no overdue books
   - Verify book copy exists and is available
   - Create checkout record with proper due date

2. **Return Processing**  
   - Find the active checkout record
   - Calculate late fees for overdue returns
   - Update checkout status and return date

3. **Query Operations**
   - Efficiently retrieve user's active checkouts
   - Find available copies (not currently checked out)
   - Determine if user has overdue books

### Implementation Requirements

- Use **src/data/Repository.cs** to access the database (a thin & basic wrapper about Dapper)
- Implement proper error handling and validation
- Return meaningful error messages for business rule violations

## Demonstration

Create a `Main` method that demonstrates your system:

1. Attempt to check out a book to Alice (should fail - she has overdue items)
2. Alice returns her overdue book (calculate late fee)
3. Alice successfully checks out a new book
4. Display the final state of all checkouts

## Evaluation Focus

This interview evaluates:

**Domain Modeling (40%)**
- Correct identification of entities and relationships
- Proper separation of concerns (books vs copies, etc.)
- Database design that enforces business rules
- Object model that accurately represents the domain

**Implementation Quality (40%)**
- Working code that handles the business requirements
- Proper error handling and validation
- Efficient database queries
- Clean, readable code structure

**Design Thinking (20%)**
- Explanation of design choices and trade-offs
- Adaptability to new requirements
- Understanding of potential issues and solutions

**Focus Areas:**
Remember, this interview is about **modeling skills**, not memorizing syntax. 

Focus on:
- Getting the domain model right
- Making good design decisions
- Explaining your thought process
- Writing code that works

Good luck! Take time to think through the design before jumping into implementation.
