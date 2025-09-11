namespace LibraryManagement.Models;

/// <summary>
/// Result returned from return operations
/// </summary>
public class ReturnResult
{
    public bool Success { get; set; }
    public decimal LateFee { get; set; }
    public int DaysOverdue { get; set; }
}