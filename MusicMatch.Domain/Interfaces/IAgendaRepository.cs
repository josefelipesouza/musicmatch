using MusicMatch.Domain.Entities;
using MusicMatch.Domain.Enums;

namespace MusicMatch.Domain.Interfaces;

public interface IAgendaRepository
{
    Task<IEnumerable<Agenda>> GetByArtistaIdAsync(Guid artistaId);
    Task<Agenda?> GetByIdAsync(Guid id);
    Task AddAsync(Agenda agenda);
    Task UpdateAsync(Agenda agenda);
    Task<IEnumerable<Agenda>> BuscarPorFiltrosAsync(
        FormatoShow formatoShow,
        double latitudeContratante,
        double longitudeContratante,
        double raioKm,
        DateTime data,
        TimeSpan horarioInicio,
        TimeSpan horarioFim,
        bool? equipamentoProprio,
        decimal? baseCacheHoraAte);
}