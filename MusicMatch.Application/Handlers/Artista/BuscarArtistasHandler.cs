using MediatR;
using MusicMatch.Application.DTOs;
using MusicMatch.Application.Queries.Artista;
using MusicMatch.Domain.Interfaces;
using MusicMatch.Domain.Services;

namespace MusicMatch.Application.Handlers.Artista;

public class BuscarArtistasHandler : IRequestHandler<BuscarArtistasQuery, List<MatchDto>>
{
    private readonly IAgendaRepository _repository;

    public BuscarArtistasHandler(IAgendaRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<MatchDto>> Handle(BuscarArtistasQuery request, CancellationToken cancellationToken)
    {
        var agendas = await _repository.BuscarPorFiltrosAsync(
            request.FormatoShow,
            request.Latitude,
            request.Longitude,
            request.RaioKm,
            request.Data,
            request.HorarioInicio,
            request.HorarioFim,
            request.EquipamentoProprio,
            request.BaseCacheHoraAte
        );

        return agendas
        .Select(a => new MatchDto
        {
            AgendaId = a.Id,
            ArtistaId = a.ArtistaId,
            Nome = a.Artista?.Nome ?? string.Empty,
            RazaoSocial = a.Artista?.RazaoSocial,
            Cidade = a.Cidade,
            DistanciaKm = Math.Round(
                GeoCalculator.CalcularDistanciaKm(
                    request.Latitude, request.Longitude,
                    a.Latitude, a.Longitude), 1),
            EquipamentoProprio = a.EquipamentoProprio,
            FormatosShow = [a.FormatoShow.ToString()],
            BaseCacheHora = a.BaseCacheHora,
            Celular1 = a.Artista?.Celular1 ?? string.Empty,
            Celular2 = a.Artista?.Celular2,
        })
        .OrderBy(m => m.DistanciaKm)
        .ToList();

    }
}