using Api.Interfaces;
using Api.Models;
using Api.Models.DTOs.TaskListDTOs;
using FluentValidation;

namespace Api.Validations;

public class CreateUpdateTaskListValidator : AbstractValidator<CreateTaskListDto>
{
    public CreateUpdateTaskListValidator(IBoardRepository boardRepository)
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
        RuleFor(x => x.BoardId).NotEmpty().MustAsync(async (board, cancellation) =>
        {
            var boardEntity = await boardRepository.GetById(board);
            return boardEntity != null;
        }).WithMessage("Board does not exist.");
    }
}