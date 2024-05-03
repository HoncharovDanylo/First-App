
using Api.Models;

namespace Api.Interfaces;

public interface IBoardRepository
{
    Task<IEnumerable<Board?>> GetAll();
    Task<Board?> GetById(int id);
    Task Create(Board board);
    Task Update(Board board);
    Task Delete(Board board);
    
}