using MediatR;
using MusicMatch.Application.DTOs;
using MusicMatch.Application.Queries.Artista;
using MusicMatch.Domain.Interfaces;

namespace MusicMatch.Application.Handlers.Artista;

public class GetArtistaByIdHandler : IRequestHandler<GetArtistaByIdQuery, ArtistaDto?>
{
    private readonly IArtistaRepository _repository;

    public GetArtistaByIdHandler(IArtistaRepository repository)
    {
        _repository = repository;
    }

    public async Task<ArtistaDto?> Handle(GetArtistaByIdQuery request, CancellationToken cancellationToken)
    {
        var artista = await _repository.GetByIdAsync(request.Id);
        if (artista is null) return null;

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