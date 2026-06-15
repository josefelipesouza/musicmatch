namespace MusicMatch.Application.DTOs;

public class EventoDto
{
    public Guid Id { get; set; }
    public Guid ContratanteId { get; set; }
    public string Localizacao { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double RaioKm { get; set; }
    public int FormatoShow { get; set; }
    public int Tipo { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
    public TimeSpan HorarioInicio { get; set; }
    public TimeSpan HorarioFim { get; set; }
    public bool EquipamentoProprio { get; set; }
    public decimal BaseCacheHoraAte { get; set; }
    public DateTime CriadoEm { get; set; }
}