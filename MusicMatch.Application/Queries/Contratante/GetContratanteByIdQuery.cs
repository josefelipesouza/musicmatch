using MediatR;
using MusicMatch.Application.DTOs;

namespace MusicMatch.Application.Queries.Contratante;

public record GetContratanteByIdQuery(Guid Id) : IRequest<ContratanteDto?>;