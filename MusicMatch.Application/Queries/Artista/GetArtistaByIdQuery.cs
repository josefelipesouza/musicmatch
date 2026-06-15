using MediatR;
using MusicMatch.Application.DTOs;

namespace MusicMatch.Application.Queries.Artista;

public record GetArtistaByIdQuery(Guid Id) : IRequest<ArtistaDto?>;