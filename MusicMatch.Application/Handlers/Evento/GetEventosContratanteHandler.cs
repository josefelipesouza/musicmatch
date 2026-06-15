using MediatR;
using MusicMatch.Application.DTOs;
using MusicMatch.Application.Queries.Evento;
using MusicMatch.Domain.Interfaces;

namespace MusicMatch.Application.Handlers.Evento;

public class GetEventosContratanteHandler : IRequestHandler<GetEventosByContratanteQuery, List<EventoDto>>
{
    private readonly IEventoRepository _repository;

    public GetEventosContratanteHandler(IEventoRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<EventoDto>> Handle(GetEventosByContratanteQuery request, CancellationToken cancellationToken)
    {
        var eventos = await _repository.GetByContratanteAsync(request.ContratanteId);

        return eventos
            .OrderBy(e => e.DataInicio)
            .ThenBy(e => e.HorarioInicio)
            .Select(e => new EventoDto
            {
                Id = e.Id,
                ContratanteId = e.ContratanteId,
                Localizacao = e.Localizacao,
                Latitude = e.Latitude,
                Longitude = e.Longitude,
                RaioKm = e.RaioKm,
                FormatoShow = (int)e.FormatoShow,
                Tipo = (int)e.Tipo,
                DataInicio = e.DataInicio,
                DataFim = e.DataFim,
                HorarioInicio = e.HorarioInicio,
                HorarioFim = e.HorarioFim,
                EquipamentoProprio = e.EquipamentoProprio,
                BaseCacheHoraAte = e.BaseCacheHoraAte,
                CriadoEm = e.CriadoEm
            })
            .ToList();
    }
}