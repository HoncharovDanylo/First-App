using Api.Enums;
using Api.Interfaces;
using Api.Models.DTOs.CardDTOs;
using FluentValidation;

namespace Api.Validations;

public class CreateUpdateCardValidator : AbstractValidator<CreateUpdateCardDto>
{
    public CreateUpdateCardValidator(ITaskListRepository taskListRepository)
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Priority).IsEnumName(typeof(PriorityEnum), caseSensitive: false);
        RuleFor(x => x.Description).MaximumLength(1000);
        RuleFor(x => x.DueDate).NotEmpty().GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now).ToDateTime(TimeOnly.Parse("00:00 AM")));
        RuleFor(x => x.TaskListId).NotEmpty().MustAsync(async (list,cancelation) =>
        {
            var taskList = await taskListRepository.GetById(list);
            return taskList != null;
            
        }).WithMessage("Task List does not exist.");
    }
}