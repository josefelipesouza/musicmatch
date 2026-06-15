using Microsoft.EntityFrameworkCore;
using MusicMatch.Domain.Entities;
using MusicMatch.Domain.Interfaces;
using MusicMatch.Infrastructure.Persistence;

namespace MusicMatch.Infrastructure.Repositories;

public class ContratanteRepository : IContratanteRepository
{
    private readonly AppDbContext _context;

    public ContratanteRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Contratante?> GetByIdAsync(Guid id)
    {
        return await _context.Contratantes
            .Include(c => c.Eventos)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Contratante?> GetByEmailAsync(string email)
    {
        return await _context.Contratantes
            .FirstOrDefaultAsync(c => c.Email == email);
    }

    public async Task AddAsync(Contratante contratante)
    {
        await _context.Contratantes.AddAsync(contratante);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Contratante contratante)
    {
        _context.Contratantes.Update(contratante);
        await _context.SaveChangesAsync();
    }
}