using System.ComponentModel.DataAnnotations;
using Api.Models.DTOs.CardDTOs;

namespace Api.Models.DTOs.TaskListDTOs;

public class TaskListDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int CardsCount { get; set; }
    public int BoardId { get; set; }
    public List<CardDto> Cards { get; set; }
}