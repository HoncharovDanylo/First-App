using Api.Enums;
using Api.Models.DTOs.CardDTOs;
using FluentValidation;

namespace Api.Validations;

public class CreateUpdateCardValidator : AbstractValidator<CreateUpdateCardDto>
{
    public CreateUpdateCardValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Priority).IsEnumName(typeof(PriorityEnum), caseSensitive: false);
        RuleFor(x => x.Description).MaximumLength(1000);
        RuleFor(x => x.DueDate).NotEmpty().GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now));
        RuleFor(x => x.TaskListId).NotEmpty();
    }
}