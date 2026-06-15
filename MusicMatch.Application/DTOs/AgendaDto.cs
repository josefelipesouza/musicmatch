using MusicMatch.Domain.Enums;

namespace MusicMatch.Application.DTOs;

public class AgendaDto
{
    public Guid Id { get; set; }
    public Guid ArtistaId { get; set; }
    public string FormatoShow { get; set; } = string.Empty;
    public bool EquipamentoProprio { get; set; }
    public bool Disponivel { get; set; }
    public DateTime Data { get; set; }
    public TimeSpan HorarioInicial { get; set; }
    public TimeSpan HorarioFinal { get; set; }
    public decimal BaseCacheHora { get; set; }
    public string Cidade { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}