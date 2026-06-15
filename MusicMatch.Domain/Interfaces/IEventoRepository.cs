using MusicMatch.Domain.Entities;

namespace MusicMatch.Domain.Interfaces;

public interface IEventoRepository
{
    Task<Evento?> GetByIdAsync(Guid id);
    Task<IEnumerable<Evento>> GetByContratanteAsync(Guid contratanteId);
    Task AddAsync(Evento evento);
    Task UpdateAsync(Evento evento);
    Task DeleteAsync(Evento evento);
}