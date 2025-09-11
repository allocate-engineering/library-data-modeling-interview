namespace LibraryManagement.Models;

/// <summary>
/// Result returned from checkout operations
/// </summary>
public class CheckoutResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime? DueDate { get; set; }
}