using MediatR;
using MusicMatch.Application.Commands.Artista;
using MusicMatch.Application.DTOs;
using MusicMatch.Domain.Entities;
using MusicMatch.Domain.Interfaces;

namespace MusicMatch.Application.Handlers.Artista;

public class CadastrarAgendaHandler : IRequestHandler<CadastrarAgendaCommand, AgendaDto>
{
    private readonly IAgendaRepository _repository;

    public CadastrarAgendaHandler(IAgendaRepository repository)
    {
        _repository = repository;
    }

    public async Task<AgendaDto> Handle(CadastrarAgendaCommand request, CancellationToken cancellationToken)
    {
        var agenda = new Agenda(
            request.ArtistaId,
            request.FormatoShow,
            request.EquipamentoProprio,
            request.Data,
            request.HorarioInicial,
            request.HorarioFinal,
            request.BaseCacheHora,
            request.Cidade,
            request.Latitude,
            request.Longitude
        );

        await _repository.AddAsync(agenda);

        return new AgendaDto
        {
            Id = agenda.Id,
            ArtistaId = agenda.ArtistaId,
            FormatoShow = agenda.FormatoShow.ToString(),
            EquipamentoProprio = agenda.EquipamentoProprio,
            Disponivel = agenda.Disponivel,
            Data = agenda.Data,
            HorarioInicial = agenda.HorarioInicial,
            HorarioFinal = agenda.HorarioFinal,
            BaseCacheHora = agenda.BaseCacheHora,
            Cidade = agenda.Cidade,
            Latitude = agenda.Latitude,
            Longitude = agenda.Longitude
        };
    }
}