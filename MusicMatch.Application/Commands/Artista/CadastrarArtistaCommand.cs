using MediatR;
using MusicMatch.Application.DTOs;

namespace MusicMatch.Application.Commands.Artista;

public record CadastrarArtistaCommand(
    string Email,
    string Nome,
    string CpfCnpj,
    string? RazaoSocial,
    string Celular1,
    string? Celular2
) : IRequest<ArtistaDto>;