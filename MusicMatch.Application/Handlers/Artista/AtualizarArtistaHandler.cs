using MediatR;
using MusicMatch.Application.Commands.Artista;
using MusicMatch.Application.DTOs;
using MusicMatch.Domain.Interfaces;

namespace MusicMatch.Application.Handlers.Artista;

public class AtualizarArtistaHandler : IRequestHandler<AtualizarArtistaCommand, ArtistaDto>
{
    private readonly IArtistaRepository _repository;

    public AtualizarArtistaHandler(IArtistaRepository repository)
    {
        _repository = repository;
    }

    public async Task<ArtistaDto> Handle(AtualizarArtistaCommand request, CancellationToken cancellationToken)
    {
        var artista = await _repository.GetByIdAsync(request.Id)
            ?? throw new Exception("Artista não encontrado.");

        artista.AtualizarDados(request.CpfCnpj, request.RazaoSocial, request.Celular1, request.Celular2);
        await _repository.UpdateAsync(artista);

        return new ArtistaDto
        {
            Id = artista.Id,
            Email = artista.Email,
            Nome = artista.Nome,
            CpfCnpj = artista.CpfCnpj,
            RazaoSocial = artista.RazaoSocial,
            Celular1 = artista.Celular1,
            Celular2 = artista.Celular2,
        };
    }
}