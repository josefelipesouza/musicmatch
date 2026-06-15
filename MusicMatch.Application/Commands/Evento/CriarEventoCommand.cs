using MediatR;
using MusicMatch.Application.DTOs;
using MusicMatch.Domain.Enums;

namespace MusicMatch.Application.Commands.Evento;

public record CriarEventoCommand(
    Guid ContratanteId,
    string Localizacao,
    double Latitude,
    double Longitude,
    double RaioKm,
    FormatoShow FormatoShow,
    TipoEvento Tipo,
    DateTime DataInicio,
    DateTime DataFim,
    TimeSpan HorarioInicio,
    TimeSpan HorarioFim,
    bool EquipamentoProprio,
    decimal BaseCacheHoraAte
) : IRequest<EventoDto>;