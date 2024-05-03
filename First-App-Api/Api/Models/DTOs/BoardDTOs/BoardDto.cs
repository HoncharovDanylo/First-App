using Api.Models.DTOs.HistoryDTOs;
using Api.Models.DTOs.TaskListDTOs;

namespace Api.Models.DTOs.BoardDTOs;

public class BoardDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<TaskListDto> TaskLists { get; set; }
    public List<HistoryDto> Histories { get; set; }
}