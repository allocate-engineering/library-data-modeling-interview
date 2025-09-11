using Npgsql;

namespace LibraryManagement;

public class DatabaseConnection
{
    private readonly string _connectionString;

    public DatabaseConnection()
    {
        // TODO: Move to configuration
        _connectionString = "Host=postgres;Database=librarydb;Username=librarian;Password=bookworm123";
    }

    public NpgsqlConnection GetConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }
}