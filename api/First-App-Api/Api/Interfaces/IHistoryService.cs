using Api.Models;
using Api.Models.DTOs.CardDTOs;

namespace Api.Interfaces;

public interface IHistoryService
{
    History TrackCreation(Card card, string listName);
    History TrackDeletion(Card card, string listName);
    IEnumerable<History> TrackUpdate(Dictionary<string,object> oldValue, Dictionary<string,object> newValue);
}