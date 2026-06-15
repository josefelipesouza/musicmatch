using MediatR;

namespace MusicMatch.Application.Commands.Artista;

public record CancelarAgendaCommand(Guid AgendaId) : IRequest<bool>;