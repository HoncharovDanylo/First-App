using Api.Models.DTOs.BoardDTOs;
using FluentValidation;

namespace Api.Validations;

public class CreateUpdateBoardValidator : AbstractValidator<CreateUpdateBoardDto>
{
    public CreateUpdateBoardValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
    }
}