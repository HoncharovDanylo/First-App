namespace Api.Models.DTOs.HistoryDTOs;

public class HistoryDto
{
    public string CardName { get; set; }
    public string Action { get; set; }
    public string Field { get; set; }
    public string PreviousValue { get; set; }
    public string NewValue { get; set; }
    public DateTime Timestamp { get; set; }
}