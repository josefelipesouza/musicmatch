using Microsoft.EntityFrameworkCore;
using MusicMatch.Domain.Entities;
using MusicMatch.Domain.Interfaces;
using MusicMatch.Infrastructure.Persistence;

namespace MusicMatch.Infrastructure.Repositories;

public class MensagemRepository : IMensagemRepository
{
    private readonly AppDbContext _context;

    public MensagemRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Mensagem>> GetByUsuarioAsync(Guid usuarioId)
    {
        return await _context.Mensagens
            .Where(m => m.AutorId == usuarioId || m.DestinoId == usuarioId)
            .OrderByDescending(m => m.DataHora)
            .ToListAsync();
    }

    public async Task AddAsync(Mensagem mensagem)
    {
        await _context.Mensagens.AddAsync(mensagem);
        await _context.SaveChangesAsync();
    }
}