using MediatR;
using MusicMatch.Application.DTOs;

namespace MusicMatch.Application.Queries.Artista;

public record GetAgendasArtistaQuery(Guid ArtistaId) : IRequest<IEnumerable<AgendaDto>>;