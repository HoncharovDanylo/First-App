using System.ComponentModel.DataAnnotations;
namespace Api.Models.DTOs.TaskListDTOs;

public class CreateTaskListDto
{
    public string Name { get; set; }
}