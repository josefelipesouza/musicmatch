using MediatR;
using MusicMatch.Application.Commands.Contratante;
using MusicMatch.Application.DTOs;
using MusicMatch.Application.Interfaces;
using MusicMatch.Domain.Interfaces;

namespace MusicMatch.Application.Handlers.Contratante;

public class NotificarArtistaHandler : IRequestHandler<NotificarArtistaCommand, bool>
{
    private readonly IArtistaRepository _artistaRepository;
    private readonly IEventoRepository _eventoRepository;
    private readonly IContratanteRepository _contratanteRepository;
    private readonly IMessageService _messageService;

    public NotificarArtistaHandler(
        IArtistaRepository artistaRepository,
        IEventoRepository eventoRepository,
        IContratanteRepository contratanteRepository,
        IMessageService messageService)
    {
        _artistaRepository = artistaRepository;
        _eventoRepository = eventoRepository;
        _contratanteRepository = contratanteRepository;
        _messageService = messageService;
    }

    public async Task<bool> Handle(NotificarArtistaCommand request, CancellationToken cancellationToken)
    {
        var artista = await _artistaRepository.GetByIdAsync(request.ArtistaId);
        if (artista is null) return false;

        var evento = await _eventoRepository.GetByIdAsync(request.EventoId);
        if (evento is null) return false;

        var contratante = await _contratanteRepository.GetByIdAsync(request.ContratanteId);
        if (contratante is null) return false;

        var dto = new NotificarArtistaDto
        {
            ArtistaId = artista.Id.ToString(),
            ArtistaEmail = artista.Email,
            ArtistaNome = artista.Nome,
            ArtistaRazaoSocial = artista.RazaoSocial,
            ArtistaCelular1 = artista.Celular1,
            ArtistaCelular2 = artista.Celular2,
            EventoTipo = evento.Tipo.ToString(),
            EventoLocalizacao = evento.Localizacao,
            EventoData = evento.DataInicio.ToString("dd/MM/yyyy"),
            EventoHorario = $"{evento.HorarioInicio:hh\\:mm} → {evento.HorarioFim:hh\\:mm}",
            FormatoShow = evento.FormatoShow.ToString(),
            BaseCacheHoraAte = evento.BaseCacheHoraAte,
            ContratanteNome = contratante.Nome,
            ContratanteEmail = contratante.Email,
            ContratanteCelular1 = contratante.Celular1,
            ContratanteCelular2 = contratante.Celular2,
        };

        await _messageService.PublishAsync("artista.notificado", dto);
        return true;
    }
}