using System.ComponentModel.DataAnnotations;

namespace Api.Models.DTOs.CardDTOs;

public class CreateCardDto
{
    [Required]
    [Length(5,100)]
    public string Title { get; set; }
    public string Description { get; set; }
    public string Priority { get; set; }
    public DateOnly DueDate { get; set; }
    [Required]
    public int TaskListId { get; set; }
}