using Microsoft.EntityFrameworkCore;
using MusicMatch.Domain.Entities;
using MusicMatch.Domain.Interfaces;
using MusicMatch.Infrastructure.Persistence;

namespace MusicMatch.Infrastructure.Repositories;

public class EventoRepository : IEventoRepository
{
    private readonly AppDbContext _context;

    public EventoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Evento?> GetByIdAsync(Guid id)
    {
        return await _context.Eventos
            .Include(e => e.Contratante)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<Evento>> GetByContratanteAsync(Guid contratanteId)
    {
        return await _context.Eventos
            .Where(e => e.ContratanteId == contratanteId)
            .ToListAsync();
    }

    public async Task AddAsync(Evento evento)
    {
        await _context.Eventos.AddAsync(evento);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Evento evento)
    {
        _context.Eventos.Update(evento);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Evento evento)
    {
        _context.Eventos.Remove(evento);
        await _context.SaveChangesAsync();
    }
}