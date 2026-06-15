using MediatR;
using MusicMatch.Application.Commands.Evento;
using MusicMatch.Application.DTOs;
using MusicMatch.Application.Interfaces;
using MusicMatch.Domain.Interfaces;

namespace MusicMatch.Application.Handlers.Evento;

public class CriarEventoHandler : IRequestHandler<CriarEventoCommand, EventoDto>
{
    private readonly IEventoRepository _repository;
    private readonly IMessageService _messageService;

    public CriarEventoHandler(IEventoRepository repository, IMessageService messageService)
    {
        _repository = repository;
        _messageService = messageService;
    }

    public async Task<EventoDto> Handle(CriarEventoCommand request, CancellationToken cancellationToken)
    {
        var evento = new Domain.Entities.Evento(
            request.ContratanteId,
            request.Localizacao,
            request.Latitude,
            request.Longitude,
            request.RaioKm,
            request.FormatoShow,
            request.Tipo,
            request.DataInicio,
            request.DataFim,
            request.HorarioInicio,
            request.HorarioFim,
            request.EquipamentoProprio,
            request.BaseCacheHoraAte
        );

        await _repository.AddAsync(evento);

        await _messageService.PublishAsync("evento.criado", new
        {
            evento.Id,
            evento.Localizacao,
            evento.FormatoShow,
            evento.Tipo,
            evento.DataInicio,
            evento.CriadoEm
        });

        return new EventoDto
        {
            Id = evento.Id,
            ContratanteId = evento.ContratanteId,
            Localizacao = evento.Localizacao,
            Latitude = evento.Latitude,
            Longitude = evento.Longitude,
            RaioKm = evento.RaioKm,
            FormatoShow = (int)evento.FormatoShow,
            Tipo = (int)evento.Tipo,
            DataInicio = evento.DataInicio,
            DataFim = evento.DataFim,
            HorarioInicio = evento.HorarioInicio,
            HorarioFim = evento.HorarioFim,
            EquipamentoProprio = evento.EquipamentoProprio,
            BaseCacheHoraAte = evento.BaseCacheHoraAte,
            CriadoEm = evento.CriadoEm
        };
    }
}