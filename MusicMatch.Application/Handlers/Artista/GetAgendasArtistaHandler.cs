using MediatR;
using MusicMatch.Application.DTOs;
using MusicMatch.Application.Queries.Artista;
using MusicMatch.Domain.Interfaces;

namespace MusicMatch.Application.Handlers.Artista;

public class GetAgendasArtistaHandler : IRequestHandler<GetAgendasArtistaQuery, IEnumerable<AgendaDto>>
{
    private readonly IAgendaRepository _repository;

    public GetAgendasArtistaHandler(IAgendaRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<AgendaDto>> Handle(GetAgendasArtistaQuery request, CancellationToken cancellationToken)
    {
        var agendas = await _repository.GetByArtistaIdAsync(request.ArtistaId);

        return agendas
            .OrderBy(a => a.Data)
            .ThenBy(a => a.HorarioInicial)
            .Select(a => new AgendaDto
            {
                Id = a.Id,
                ArtistaId = a.ArtistaId,
                FormatoShow = a.FormatoShow.ToString(),
                EquipamentoProprio = a.EquipamentoProprio,
                Disponivel = a.Disponivel,
                Data = a.Data,
                HorarioInicial = a.HorarioInicial,
                HorarioFinal = a.HorarioFinal,
                BaseCacheHora = a.BaseCacheHora,
                Cidade = a.Cidade,
                Latitude = a.Latitude,
                Longitude = a.Longitude
            });
    }
}