using Api.Models;

namespace Api.Interfaces;

public interface IHistoryRepository
{
    Task<IEnumerable<History>> GetAll(int page);
    Task<IEnumerable<History>> GetByCardId(int cardId);
}