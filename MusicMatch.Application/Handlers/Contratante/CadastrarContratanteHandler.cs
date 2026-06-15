using MediatR;
using MusicMatch.Application.Commands.Contratante;
using MusicMatch.Application.DTOs;
using MusicMatch.Application.Interfaces;
using MusicMatch.Domain.Interfaces;

namespace MusicMatch.Application.Handlers.Contratante;

public class CadastrarContratanteHandler : IRequestHandler<CadastrarContratanteCommand, ContratanteDto>
{
    private readonly IContratanteRepository _repository;
    private readonly IMessageService _messageService;

    public CadastrarContratanteHandler(IContratanteRepository repository, IMessageService messageService)
    {
        _repository = repository;
        _messageService = messageService;
    }

    public async Task<ContratanteDto> Handle(CadastrarContratanteCommand request, CancellationToken cancellationToken)
    {
        var contratante = new Domain.Entities.Contratante(
            request.Email,
            request.Nome,
            request.CpfCnpj,
            request.RazaoSocial,
            request.Celular1,
            request.Celular2
        );

        await _repository.AddAsync(contratante);

        await _messageService.PublishAsync("contratante.cadastrado", new
        {
            contratante.Id,
            contratante.Nome,
            contratante.Email,
            contratante.CriadoEm
        });

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