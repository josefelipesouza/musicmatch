namespace MusicMatch.Application.DTOs;

public class NotificarArtistaDto
{
    public string ArtistaId { get; set; } = string.Empty;
    public string ArtistaEmail { get; set; } = string.Empty;
    public string ArtistaNome { get; set; } = string.Empty;
    public string? ArtistaRazaoSocial { get; set; }
    public string ArtistaCelular1 { get; set; } = string.Empty;
    public string? ArtistaCelular2 { get; set; }
    public string EventoTipo { get; set; } = string.Empty;
    public string EventoLocalizacao { get; set; } = string.Empty;
    public string EventoData { get; set; } = string.Empty;
    public string EventoHorario { get; set; } = string.Empty;
    public string FormatoShow { get; set; } = string.Empty;
    public decimal BaseCacheHoraAte { get; set; }
    public string ContratanteNome { get; set; } = string.Empty;
    public string ContratanteEmail { get; set; } = string.Empty;
    public string ContratanteCelular1 { get; set; } = string.Empty;
    public string? ContratanteCelular2 { get; set; }
}