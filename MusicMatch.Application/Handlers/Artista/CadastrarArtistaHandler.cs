using MediatR;
using MusicMatch.Application.Commands.Artista;
using MusicMatch.Application.DTOs;
using MusicMatch.Domain.Interfaces;
using ArtistaEntity = MusicMatch.Domain.Entities.Artista;

namespace MusicMatch.Application.Handlers.Artista;

public class CadastrarArtistaHandler : IRequestHandler<CadastrarArtistaCommand, ArtistaDto>
{
    private readonly IArtistaRepository _repository;

    public CadastrarArtistaHandler(IArtistaRepository repository)
    {
        _repository = repository;
    }

    public async Task<ArtistaDto> Handle(CadastrarArtistaCommand request, CancellationToken cancellationToken)
    {
        var artista = new ArtistaEntity(
            request.Email,
            request.Nome,
            request.CpfCnpj,
            request.RazaoSocial,
            request.Celular1,
            request.Celular2
        );

        await _repository.AddAsync(artista);

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