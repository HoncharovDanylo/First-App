using Api.Interfaces;
using Api.Models;
using Api.Models.DTOs.BoardDTOs;
using Api.Models.DTOs.CardDTOs;
using Api.Models.DTOs.HistoryDTOs;
using Api.Models.DTOs.TaskListDTOs;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    public class BoardController : ControllerBase
    {
        private readonly IBoardRepository _boardRepository;
        private readonly IValidator<CreateUpdateBoardDto> _validator;

        public BoardController(IBoardRepository boardRepository, IValidator<CreateUpdateBoardDto> validator)
        {
            _boardRepository = boardRepository;
            _validator = validator;
        }

        [HttpGet("boards")]
        public async Task<IActionResult> GetBoards()
        {
            var boards = (await _boardRepository.GetAll()).Select(x => new BoardDto()
            {
                Id = x.Id,
                Name = x.Name,
            });
            return Ok(boards);
        }

        [HttpGet("boards/{id}")]
        public async Task<IActionResult> GetBoard(int id)
        {
            var board = await _boardRepository.GetById(id);
            if (board == null)
            {
                return NotFound();
            }

            var boardDto = new BoardDto()
            {
                Id = board.Id,
                Name = board.Name,
            };
            return Ok(boardDto);
        }
        [HttpPost("boards/create")]
        public async Task<IActionResult> CreateBoard([FromBody] CreateUpdateBoardDto boardDto)
        {
            if ((await _validator.ValidateAsync(boardDto)).IsValid)
            {
                var board = new Board() { Name = boardDto.Name.Trim() };
                await _boardRepository.Create(board);
                return Ok(); 
            }
            return BadRequest("valid error");
        }
        
        [HttpPut("boards/update/{id}")]
        public async Task<IActionResult> UpdateBoard(int id, [FromBody] CreateUpdateBoardDto boardDto)
        {
            if ((await _validator.ValidateAsync(boardDto)).IsValid)
            {
                var board = await _boardRepository.GetById(id);
                board.Name = boardDto.Name.Trim();
                await _boardRepository.Update(board);
                return Ok();
            }
            return BadRequest("valid error");
        }
        [HttpDelete("boards/{id}")]
        public async Task<IActionResult> DeleteBoard(int id)
        {
            var board = await _boardRepository.GetById(id);
            if (board == null)
                return NotFound();
            await _boardRepository.Delete(board);
            return Ok();
        }
        
    }
}

