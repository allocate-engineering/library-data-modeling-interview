using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using Dapper;
using Npgsql;

namespace LibraryManagement.Data;

/// <summary>
/// Generic repository for basic CRUD operations using Dapper
/// Conventions:
/// - Table name matches class name (pluralized)
/// - Primary key is assumed to be "Id" property
/// - Use [Table("custom_name")] attribute to override table name
/// - Use [Column("custom_name")] attribute to override column name
/// </summary>
public class Repository<T> where T : class
{
    private readonly DatabaseConnection _dbConnection;
    private readonly string _tableName;

    public Repository(DatabaseConnection dbConnection)
    {
        _dbConnection = dbConnection;
        _tableName = GetTableName();
    }

    /// <summary>
    /// Get all records from the table
    /// </summary>
    public async Task<IEnumerable<T>> GetAllAsync()
    {
        using var connection = _dbConnection.GetConnection();
        var sql = $"SELECT * FROM \"{_tableName}\"";
        return await connection.QueryAsync<T>(sql);
    }

    /// <summary>
    /// Get a record by ID
    /// </summary>
    public async Task<T?> GetByIdAsync(int id)
    {
        using var connection = _dbConnection.GetConnection();
        var sql = $"SELECT * FROM \"{_tableName}\" WHERE Id = @Id";
        return await connection.QueryFirstOrDefaultAsync<T>(sql, new { Id = id });
    }

    /// <summary>
    /// Insert a new record and return the generated ID
    /// </summary>
    public async Task<int> InsertAsync(T entity)
    {
        using var connection = _dbConnection.GetConnection();
        
        var properties = GetProperties();
        var columns = string.Join(", ", properties.Where(p => !string.Equals(p.Name, "Id", StringComparison.OrdinalIgnoreCase)).Select(GetColumnName));
        var values = string.Join(", ", properties.Where(p => !string.Equals(p.Name, "Id", StringComparison.OrdinalIgnoreCase)).Select(p => $"@{p.Name}"));
        
        var sql = $"INSERT INTO \"{_tableName}\" ({columns}) VALUES ({values}) RETURNING Id";
        return await connection.QuerySingleAsync<int>(sql, entity);
    }

    /// <summary>
    /// Update an existing record
    /// </summary>
    public async Task<bool> UpdateAsync(T entity)
    {
        using var connection = _dbConnection.GetConnection();
        
        var properties = GetProperties();
        var setClause = string.Join(", ", properties.Where(p => !string.Equals(p.Name, "Id", StringComparison.OrdinalIgnoreCase))
            .Select(p => $"{GetColumnName(p)} = @{p.Name}"));
        
        var sql = $"UPDATE \"{_tableName}\" SET {setClause} WHERE Id = @Id";
        var rowsAffected = await connection.ExecuteAsync(sql, entity);
        return rowsAffected > 0;
    }

    /// <summary>
    /// Delete a record by ID
    /// </summary>
    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = _dbConnection.GetConnection();
        var sql = $"DELETE FROM \"{_tableName}\" WHERE Id = @Id";
        var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
        return rowsAffected > 0;
    }

    /// <summary>
    /// Execute a custom query
    /// </summary>
    public async Task<IEnumerable<T>> QueryAsync(string sql, object? parameters = null)
    {
        using var connection = _dbConnection.GetConnection();
        return await connection.QueryAsync<T>(sql, parameters);
    }

    /// <summary>
    /// Execute a custom query returning a single result
    /// </summary>
    public async Task<T?> QueryFirstOrDefaultAsync(string sql, object? parameters = null)
    {
        using var connection = _dbConnection.GetConnection();
        return await connection.QueryFirstOrDefaultAsync<T>(sql, parameters);
    }

    /// <summary>
    /// Execute a non-query command (INSERT/UPDATE/DELETE)
    /// </summary>
    public async Task<int> ExecuteAsync(string sql, object? parameters = null)
    {
        using var connection = _dbConnection.GetConnection();
        return await connection.ExecuteAsync(sql, parameters);
    }

    /// <summary>
    /// Get database connection for complex operations
    /// </summary>
    public NpgsqlConnection GetConnection()
    {
        return _dbConnection.GetConnection();
    }

    private string GetTableName()
    {
        var tableAttribute = typeof(T).GetCustomAttribute<TableAttribute>();
        if (tableAttribute != null)
            return tableAttribute.Name;

        // Use class name as table name (singular, matching C# casing)
        return typeof(T).Name;
    }

    private PropertyInfo[] GetProperties()
    {
        return typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanRead && p.CanWrite)
            .ToArray();
    }

    private string GetColumnName(PropertyInfo property)
    {
        var columnAttribute = property.GetCustomAttribute<ColumnAttribute>();
        if (columnAttribute != null)
            return columnAttribute.Name ?? property.Name;

        // Use property name as column name (matching C# casing)
        return property.Name;
    }
}