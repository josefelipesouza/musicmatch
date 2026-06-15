using MediatR;
using MusicMatch.Application.DTOs;

namespace MusicMatch.Application.Queries.Evento;

public record GetEventosByContratanteQuery(Guid ContratanteId) : IRequest<List<EventoDto>>;