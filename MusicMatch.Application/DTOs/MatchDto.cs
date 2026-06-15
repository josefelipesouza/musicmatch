namespace MusicMatch.Application.DTOs;

public class MatchDto
{
    public Guid AgendaId { get; set; }
    public Guid ArtistaId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? RazaoSocial { get; set; }
    public string Cidade { get; set; } = string.Empty;
    public double DistanciaKm { get; set; }
    public bool EquipamentoProprio { get; set; }
    public List<string> FormatosShow { get; set; } = new();
    public decimal BaseCacheHora { get; set; }
    public string Celular1 { get; set; } = string.Empty;
    public string? Celular2 { get; set; }
}