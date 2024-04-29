using System.ComponentModel.DataAnnotations;

namespace Api.Models.DTOs.CardDTOs;

public class CreateUpdateCardDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Priority { get; set; }
    public DateOnly DueDate { get; set; }
    public int TaskListId { get; set; }
}