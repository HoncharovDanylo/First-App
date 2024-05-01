namespace Api.Models.DTOs.CardDTOs;

public class CardDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Priority { get; set; }
    public DateOnly DueDate { get; set; }
    public string TaskListName { get; set; }
    public int TaskListId { get; set; }
}