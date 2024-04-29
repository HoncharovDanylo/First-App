using Api.Models.DTOs.TaskListDTOs;
using FluentValidation;

namespace Api.Validations;

public class CreateUpdateTaskListValidator : AbstractValidator<CreateTaskListDto>
{
    public CreateUpdateTaskListValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
    }
}