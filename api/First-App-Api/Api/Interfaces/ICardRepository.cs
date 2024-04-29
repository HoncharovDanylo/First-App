using Api.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace Api.Interfaces;

public interface ICardRepository
{
    Task<Card?> GetById(int id);
    Task Create(Card card);
    Task Delete(Card card);
    Task Update(Card card);
}