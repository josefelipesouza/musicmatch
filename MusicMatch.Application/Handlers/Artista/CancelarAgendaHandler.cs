using MediatR;
using MusicMatch.Application.Commands.Artista;
using MusicMatch.Domain.Interfaces;

namespace MusicMatch.Application.Handlers.Artista;

public class CancelarAgendaHandler : IRequestHandler<CancelarAgendaCommand, bool>
{
    private readonly IAgendaRepository _repository;

    public CancelarAgendaHandler(IAgendaRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(CancelarAgendaCommand request, CancellationToken cancellationToken)
    {
        var agenda = await _repository.GetByIdAsync(request.AgendaId);
        if (agenda is null) return false;

        agenda.Cancelar();
        await _repository.UpdateAsync(agenda);
        return true;
    }
}