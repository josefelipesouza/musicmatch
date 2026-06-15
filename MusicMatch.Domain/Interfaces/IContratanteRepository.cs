using MusicMatch.Domain.Entities;

namespace MusicMatch.Domain.Interfaces;

public interface IContratanteRepository
{
    Task<Contratante?> GetByIdAsync(Guid id);
    Task<Contratante?> GetByEmailAsync(string email);
    Task AddAsync(Contratante contratante);
    Task UpdateAsync(Contratante contratante);
}