using MediatR;
using MusicMatch.Application.Commands.Evento;
using MusicMatch.Domain.Interfaces;

namespace MusicMatch.Application.Handlers.Evento;

public class CancelarEventoHandler : IRequestHandler<CancelarEventoCommand, bool>
{
    private readonly IEventoRepository _repository;

    public CancelarEventoHandler(IEventoRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(CancelarEventoCommand request, CancellationToken cancellationToken)
{
    var evento = await _repository.GetByIdAsync(request.EventoId);
    Console.WriteLine($"[DELETE] EventoId={request.EventoId} | Encontrado={evento is not null}");
    if (evento is null) return false;
    await _repository.DeleteAsync(evento);
    return true;
}
}