using System.ComponentModel.DataAnnotations;
namespace Api.Models.DTOs.TaskListDTOs;

public class CreateTaskListDto
{
    [Required]
    [Length(5,100)]
    public string Name { get; set; }
}