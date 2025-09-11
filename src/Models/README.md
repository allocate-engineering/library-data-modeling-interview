# Models Directory

Create your domain model classes here based on the business requirements.

## Using the Repository

The `Repository<T>` class provides basic CRUD operations for your models:

### Example Usage

```csharp
// Example model (you'll create your own)
public class Author
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? BirthYear { get; set; }
    public string? Nationality { get; set; }
}

// Using the repository
var authorRepo = new Repository<Author>(dbConnection);

// Create
var newAuthor = new Author { Name = "George Orwell", BirthYear = 1903, Nationality = "British" };
int authorId = await authorRepo.InsertAsync(newAuthor);

// Read
var author = await authorRepo.GetByIdAsync(authorId);
var allAuthors = await authorRepo.GetAllAsync();

// Update
author.Name = "Updated Name";
await authorRepo.UpdateAsync(author);

// Delete
await authorRepo.DeleteAsync(authorId);

// Custom queries
var britishAuthors = await authorRepo.QueryAsync(
    "SELECT * FROM authors WHERE nationality = @nationality", 
    new { nationality = "British" });
```

## Table Naming Conventions

- **Class Name → Table Name**: `Author` → `Author` (exact match)
- **Override with attribute**: `[Table("custom_table_name")]`
- **Column Names**: Property names match exactly (e.g., `Name` → `Name`)
- **Override with attribute**: `[Column("custom_column")]`

## Primary Keys

- Repository assumes `Id` property is the primary key
- Auto-generated on INSERT (SERIAL in PostgreSQL)

Create your models and use the repository to focus on business logic!