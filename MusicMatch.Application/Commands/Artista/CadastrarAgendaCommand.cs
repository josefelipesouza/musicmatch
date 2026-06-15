using MediatR;
using MusicMatch.Application.DTOs;
using MusicMatch.Domain.Enums;

namespace MusicMatch.Application.Commands.Artista;

public record CadastrarAgendaCommand(
    Guid ArtistaId,
    FormatoShow FormatoShow,
    bool EquipamentoProprio,
    DateTime Data,
    TimeSpan HorarioInicial,
    TimeSpan HorarioFinal,
    decimal BaseCacheHora,
    string Cidade,
    double Latitude,
    double Longitude
) : IRequest<AgendaDto>;