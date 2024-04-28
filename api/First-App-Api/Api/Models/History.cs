namespace Api.Models;

public class History : BaseEntity
{
    public int CardId { get; set; }
    
    public string CardTitle { get; set; }
    public string Action { get; set; }
    public string Field { get; set; }
    public string? PreviousValue { get; set; }
    public string? NewValue { get; set; }
    public DateTime Timestamp { get; set; }
    
}