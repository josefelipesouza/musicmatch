using MediatR;
using MusicMatch.Application.DTOs;

namespace MusicMatch.Application.Commands.Contratante;

public record CadastrarContratanteCommand(
    string Email,
    string Nome,
    string CpfCnpj,
    string? RazaoSocial,
    string Celular1,
    string? Celular2
) : IRequest<ContratanteDto>;