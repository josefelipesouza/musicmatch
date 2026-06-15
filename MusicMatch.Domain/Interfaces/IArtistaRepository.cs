using MusicMatch.Domain.Entities;

namespace MusicMatch.Domain.Interfaces;

public interface IArtistaRepository
{
    Task<Artista?> GetByIdAsync(Guid id);
    Task<Artista?> GetByEmailAsync(string email);
    Task<IEnumerable<Artista>> GetAllAsync();
    Task AddAsync(Artista artista);
    Task UpdateAsync(Artista artista);
}