using MediatR;
using MusicMatch.Application.DTOs;

namespace MusicMatch.Application.Commands.Contratante;

public record AtualizarContratanteCommand(
    Guid Id,
    string CpfCnpj,
    string? RazaoSocial,
    string Celular1,
    string? Celular2
) : IRequest<ContratanteDto>;