using System.ComponentModel.DataAnnotations;
using Api.Models.DTOs.CardDTOs;

namespace Api.Models.DTOs.TaskListDTOs;

public class TaskListDto
{
    [Required]
    [Length(5,100)]
    public string Name { get; set; }
    public int CardsCount { get; set; }
    public List<CardDto> Cards { get; set; }
}