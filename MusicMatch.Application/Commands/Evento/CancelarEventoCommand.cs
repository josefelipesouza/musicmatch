using MediatR;

namespace MusicMatch.Application.Commands.Evento;

public record CancelarEventoCommand(Guid EventoId) : IRequest<bool>;