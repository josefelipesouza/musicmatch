using MediatR;
using MusicMatch.Application.DTOs;
using MusicMatch.Domain.Enums;

namespace MusicMatch.Application.Queries.Artista;

public record BuscarArtistasQuery(
    FormatoShow FormatoShow,
    double Latitude,
    double Longitude,
    double RaioKm,
    DateTime Data,
    TimeSpan HorarioInicio,
    TimeSpan HorarioFim,
    bool? EquipamentoProprio,
    decimal? BaseCacheHoraAte
) : IRequest<List<MatchDto>>;