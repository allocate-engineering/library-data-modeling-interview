# Library Management System - Domain Modeling Interview

A comprehensive interview question designed to test **domain modeling skills** at both the database and object-oriented programming layers.

## Overview

You're tasked with designing and implementing a library management system from scratch. This interview focuses on your ability to:

1. **Analyze complex business requirements** and identify key entities and relationships
2. **Design a robust database schema** that enforces business rules and supports efficient queries  
3. **Model object-oriented domain classes** that accurately represent the business domain
4. **Implement business logic** that handles real-world constraints and edge cases

**Time Allocation:** 90 minutes total
- **Part 1: Domain Analysis & Design (30 minutes)** - Schema + Object Model Design
- **Part 2: Implementation (45 minutes)** - Code the models and business logic
- **Part 3: Extension & Discussion (15 minutes)** - Adapt design to new requirements

## Getting Started

### Prerequisites
- Visual Studio Code with Dev Containers extension
- Docker Desktop

### Setup Instructions

1. **Open in Dev Container**
   - Open this project in VS Code
   - When prompted, select "Reopen in Container" 
   - Wait for the container to build and start

2. **Install the recommended markdown preview extension**
   - This will allow you to more easily read this README file.

3. **Access Database**
   
   **pgAdmin (Web Interface)**
   - **pgAdmin**: [http://localhost:5050](http://localhost:5050)
     - Username: `admin@example.com`
     - Password: `librarypass`
   - **Register Database in pgAdmin**:
     - The connection to your local DB should be auto-registered and ready for you when you login.,
     - Just in case, here are the connection details
         - Database: `librarydb`
         - Username: `librarian` 
         - Password: `bookworm123`
         - Port: `5432`

4. **Verify Setup (at the root of the solution)**
   ```bash   
   # Build C# project
   dotnet build
   ```

5. **After Designing Your Schema** (Part 1 of interview)
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
- Users have unique library card numbers, names, emails, membership dates
- Track user status (active/inactive)
- Users can have multiple books checked out simultaneously

**Checkout System**
- Users check out specific physical copies, not just "books"
- Checkout period is 14 days from checkout date
- Track checkout date, due date, and return date
- Calculate late fees at $0.50 per day overdue
- Keep historical record of all checkouts (past and current)

**Business Rules**
- A physical copy can only be checked out by one user at a time
- Users with overdue books cannot check out additional books
- Books must be returned before checking out new ones if user has overdue items
- System must prevent double-booking of the same physical copy

### Non-Functional Requirements
- Support efficient queries for common operations (user's books, overdue books, popular authors)
- Handle concurrent access (multiple librarians using system)
- Maintain data integrity and referential consistency
- Support future extensions (holds, renewals, reservations)

## Part 1: Domain Analysis & Design (30 minutes)

### Your Tasks

**Step 1: Entity Identification (10 minutes)**
- Identify the core domain entities and their attributes
- Map out the relationships between entities (1:1, 1:many, many:many)
- Consider the distinction between logical concepts (books) vs physical items (copies)

**Step 2: Database Schema Design (10 minutes)**  
- Design tables with appropriate columns, data types, and constraints
- Define primary keys, foreign keys, and unique constraints
- Add indexes for common query patterns
- Implement the schema in `db/library-schema.sql`

**Step 3: Object Model Design (10 minutes)**
- Design C# classes that represent your domain entities
- Define properties, relationships, and navigation patterns
- Consider how your objects will map to database tables
- Implement models in `src/Models/` directory

### Design Considerations

As you design, think about these questions:

**Database Design:**
- How do you model the book vs copy distinction?
- What constraints prevent invalid states (e.g., negative copy numbers)?
- How do you efficiently find all overdue checkouts?
- What indexes improve query performance for common operations?

**Object Model:**
- How do you represent relationships between entities?
- What validation logic belongs in your domain objects?
- How do you handle optional vs required properties?
- What methods make sense on each domain class?

### Sample Data Requirements

After completing your schema, populate it with this test data:

**Books:**
- "The Great Gatsby" (ISBN: 9780743273565, 1925) by F. Scott Fitzgerald (American, 1896)
- "1984" (ISBN: 9780451524935, 1949) by George Orwell (British, 1903)  
- "Good Omens" (ISBN: 9780060853983, 1990) by Terry Pratchett (British, 1948) & Neil Gaiman (British, 1960)

**Users:**
- Alice Johnson (Card: 12345, joined: 2023-01-15)
- Bob Smith (Card: 67890, joined: 2023-03-22)
- Charlie Brown (Card: 11111, joined: 2023-06-10)

**Current State (as of 2025-09-08):**
- Alice has "1984" copy 1 checked out (overdue since 2025-09-08)
- Bob has "The Great Gatsby" copy 1 checked out (due 2025-09-15)
- All books have 2 physical copies available

## Part 2: Implementation (45 minutes)

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

- Use **Entity Framework, Dapper, or raw SQL** - your choice
- Implement proper error handling and validation
- Return meaningful error messages for business rule violations
- Write efficient database queries (avoid N+1 problems)
- Handle edge cases (missing users, invalid ISBNs, etc.)

## Part 3: Extension & Discussion (15 minutes)

After completing your basic implementation, you'll be presented with a new requirement to test how well your design adapts to change:

### Extension Scenario (Details will be provided during interview)

**Examples of potential extensions:**
- Add book reservations/holds
- Support renewals of checkouts  
- Add book genres and categories
- Support multiple library branches
- Add fine payment tracking

### Discussion Topics

Be prepared to discuss:
- **Design Trade-offs**: Why did you choose your particular approach?
- **Alternative Designs**: What other ways could you model this domain?
- **Scalability**: How would your design handle 100,000 books and users?
- **Data Integrity**: What could go wrong and how do you prevent it?
- **Query Performance**: What are the bottlenecks in your design?

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

## Success Criteria

**Minimum Viable Solution:**
- ✅ Correct database schema with proper relationships
- ✅ Domain objects that map to your schema
- ✅ Core checkout/return logic that enforces business rules
- ✅ Working demonstration of the key workflow

**Strong Solution:**
- ✅ All of the above, plus:
- ✅ Efficient queries with appropriate indexing
- ✅ Comprehensive error handling
- ✅ Clean separation between data access and business logic
- ✅ Good discussion of design alternatives

**Excellent Solution:**
- ✅ All of the above, plus:
- ✅ Design that easily adapts to extension requirements
- ✅ Advanced database features (transactions, constraints)
- ✅ Sophisticated object model with domain methods
- ✅ Deep understanding of trade-offs and potential improvements

## Getting Help

**Database Issues:**
```bash
# Connect to database directly
psql -h postgres -U librarian -d librarydb

# View your tables
\dt

# Check your data
SELECT * FROM your_table_name;
```

**Clear Database and Start Fresh:**
If you get Liquibase checksum errors, clear everything and start over:
```bash
# Method 1: Clear Liquibase history and re-run
cd db
liquibase clear-checksums
liquibase update

# Method 2: Reset entire database (nuclear option)
psql -h postgres -U librarian -d librarydb -c "DROP SCHEMA public CASCADE; CREATE SCHEMA public;"
cd db
liquibase update
```

**Devcontainer Issues:**
If the devcontainer fails to start due to Liquibase errors:
```bash
# Rebuild the container completely
# In VS Code: Ctrl+Shift+P -> "Dev Containers: Rebuild Container"
# Or from terminal: 
docker-compose down --volumes
docker-compose up -d
```

**pgAdmin Server Connection Missing:**
If pgAdmin doesn't show the pre-configured "Library Database" server:
```bash
# Clear pgAdmin configuration and restart
docker-compose down
docker volume rm devcontainer_pga-data
docker-compose up -d
```

**Build Issues:**
```bash
# Restore and build
dotnet restore
dotnet build

# Run your program
dotnet run --project src
```

**Focus Areas:**
Remember, this interview is about **modeling skills**, not memorizing syntax. Focus on:
- Getting the domain model right
- Making good design decisions
- Explaining your thought process
- Writing code that works

Good luck! Take time to think through the design before jumping into implementation.