using MediatR;
using MusicMatch.Application.DTOs;
using MusicMatch.Application.Queries.Contratante;
using MusicMatch.Domain.Interfaces;

namespace MusicMatch.Application.Handlers.Contratante;

public class GetContratanteByIdHandler : IRequestHandler<GetContratanteByIdQuery, ContratanteDto?>
{
    private readonly IContratanteRepository _repository;

    public GetContratanteByIdHandler(IContratanteRepository repository)
    {
        _repository = repository;
    }

    public async Task<ContratanteDto?> Handle(GetContratanteByIdQuery request, CancellationToken cancellationToken)
    {
        var contratante = await _repository.GetByIdAsync(request.Id);
        if (contratante is null) return null;

        return new ContratanteDto
        {
            Id = contratante.Id,
            Email = contratante.Email,
            Nome = contratante.Nome,
            CpfCnpj = contratante.CpfCnpj,
            RazaoSocial = contratante.RazaoSocial,
            CriadoEm = contratante.CriadoEm
        };
    }
}