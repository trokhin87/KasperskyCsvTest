namespace Domain.Models;

public class ErrorRecord
{
    public DateTimeOffset Timestamp { get; init; }
    public Severity Severity { get; init; }
    public string? Product { get; init; }
    public string? Version { get; init; }
    public string? ErrorCode { get; init; }  
}