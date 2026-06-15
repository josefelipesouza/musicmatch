using Microsoft.EntityFrameworkCore;
using MusicMatch.Domain.Entities;
using MusicMatch.Domain.Interfaces;
using MusicMatch.Infrastructure.Persistence;

namespace MusicMatch.Infrastructure.Repositories;

public class ArtistaRepository : IArtistaRepository
{
    private readonly AppDbContext _context;

    public ArtistaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Artista?> GetByIdAsync(Guid id)
    {
        return await _context.Artistas
            .Include(a => a.Agendas)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Artista?> GetByEmailAsync(string email)
    {
        return await _context.Artistas
            .FirstOrDefaultAsync(a => a.Email == email);
    }

    public async Task<IEnumerable<Artista>> GetAllAsync()
    {
        return await _context.Artistas
            .Include(a => a.Agendas)
            .ToListAsync();
    }

    public async Task AddAsync(Artista artista)
    {
        await _context.Artistas.AddAsync(artista);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Artista artista)
    {
        _context.Artistas.Update(artista);
        await _context.SaveChangesAsync();
    }
}