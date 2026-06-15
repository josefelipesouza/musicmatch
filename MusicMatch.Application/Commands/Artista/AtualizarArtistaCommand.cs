using MediatR;
using MusicMatch.Application.DTOs;

namespace MusicMatch.Application.Commands.Artista;

public record AtualizarArtistaCommand(
    Guid Id,
    string CpfCnpj,
    string? RazaoSocial,
    string Celular1,
    string? Celular2
) : IRequest<ArtistaDto>;