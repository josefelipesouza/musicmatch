using Microsoft.EntityFrameworkCore;
using MusicMatch.Domain.Entities;
using MusicMatch.Domain.Enums;
using MusicMatch.Domain.Interfaces;
using MusicMatch.Domain.Services;
using MusicMatch.Infrastructure.Persistence;

namespace MusicMatch.Infrastructure.Repositories;

public class AgendaRepository : IAgendaRepository
{
    private readonly AppDbContext _context;

    public AgendaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Agenda>> GetByArtistaIdAsync(Guid artistaId)
    {
        return await _context.Agendas
            .Where(a => a.ArtistaId == artistaId)
            .OrderByDescending(a => a.Data)
            .ThenByDescending(a => a.HorarioInicial)
            .ToListAsync();
    }

    public async Task<Agenda?> GetByIdAsync(Guid id)
    {
        return await _context.Agendas.FindAsync(id);
    }

    public async Task AddAsync(Agenda agenda)
    {
        await _context.Agendas.AddAsync(agenda);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Agenda agenda)
    {
        _context.Agendas.Update(agenda);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Agenda>> BuscarPorFiltrosAsync(
        FormatoShow formatoShow,
        double latitudeContratante,
        double longitudeContratante,
        double raioKm,
        DateTime data,
        TimeSpan horarioInicio,
        TimeSpan horarioFim,
        bool? equipamentoProprio,
        decimal? baseCacheHoraAte)
    {
        var query = _context.Agendas
            .Include(a => a.Artista)   // ← garante que Artista é carregado
            .Where(a =>
                a.Disponivel &&
                a.FormatoShow == formatoShow &&
                a.Data.Date == data.Date &&
                a.HorarioInicial <= horarioInicio &&
                a.HorarioFinal >= horarioFim);

        if (equipamentoProprio.HasValue)
            query = query.Where(a => a.EquipamentoProprio == equipamentoProprio.Value);

        if (baseCacheHoraAte.HasValue)
            query = query.Where(a => a.BaseCacheHora <= baseCacheHoraAte.Value);

        var agendas = await query.ToListAsync();

        return agendas.Where(a =>
            GeoCalculator.CalcularDistanciaKm(
                latitudeContratante, longitudeContratante,
                a.Latitude, a.Longitude) <= raioKm);
    }
}