using MediatR;
using MusicMatch.Application.Commands.Contratante;
using MusicMatch.Application.DTOs;
using MusicMatch.Domain.Interfaces;

namespace MusicMatch.Application.Handlers.Contratante;

public class AtualizarContratanteHandler : IRequestHandler<AtualizarContratanteCommand, ContratanteDto>
{
    private readonly IContratanteRepository _repository;

    public AtualizarContratanteHandler(IContratanteRepository repository)
    {
        _repository = repository;
    }

    public async Task<ContratanteDto> Handle(AtualizarContratanteCommand request, CancellationToken cancellationToken)
    {
        var contratante = await _repository.GetByIdAsync(request.Id)
            ?? throw new Exception("Contratante não encontrado.");

        contratante.AtualizarDados(request.CpfCnpj, request.RazaoSocial, request.Celular1, request.Celular2);
        await _repository.UpdateAsync(contratante);

        return new ContratanteDto
        {
            Id = contratante.Id,
            Email = contratante.Email,
            Nome = contratante.Nome,
            CpfCnpj = contratante.CpfCnpj,
            RazaoSocial = contratante.RazaoSocial,
            Celular1 = contratante.Celular1,
            Celular2 = contratante.Celular2,
            CriadoEm = contratante.CriadoEm
        };
    }
}