using Api.Models;

namespace Api.Interfaces;

public interface IHistoryRepository
{
    Task<IEnumerable<History>> GetByBoard(int boardId,int page);
    Task<IEnumerable<History>> GetByCardId(int cardId);
}