using MediatR;

namespace MusicMatch.Application.Commands.Contratante;

public record NotificarArtistaCommand(
    Guid ArtistaId,
    Guid EventoId,
    Guid ContratanteId
) : IRequest<bool>;