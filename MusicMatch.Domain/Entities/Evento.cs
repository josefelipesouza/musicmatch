using MusicMatch.Domain.Enums;

namespace MusicMatch.Domain.Entities;

public class Evento
{
    public Guid Id { get; private set; }
    public Guid ContratanteId { get; private set; }
    public string Localizacao { get; private set; }
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }
    public double RaioKm { get; private set; }
    public FormatoShow FormatoShow { get; private set; }
    public TipoEvento Tipo { get; private set; }
    public DateTime DataInicio { get; private set; }
    public DateTime DataFim { get; private set; }
    public TimeSpan HorarioInicio { get; private set; }
    public TimeSpan HorarioFim { get; private set; }
    public bool EquipamentoProprio { get; private set; }
    public decimal BaseCacheHoraAte { get; private set; }
    public Contratante Contratante { get; private set; } = null!;
    public DateTime CriadoEm { get; private set; }

    private Evento() { Localizacao = null!; }

    public Evento(Guid contratanteId, string localizacao,
        double latitude, double longitude, double raioKm,
        FormatoShow formatoShow, TipoEvento tipo,
        DateTime dataInicio, DateTime dataFim,
        TimeSpan horarioInicio, TimeSpan horarioFim,
        bool equipamentoProprio, decimal baseCacheHoraAte)
    {
        if (string.IsNullOrWhiteSpace(localizacao))
            throw new ArgumentException("Localização é obrigatória.");
        if (dataFim < dataInicio)
            throw new ArgumentException("Data fim não pode ser anterior à data início.");
        if (raioKm <= 0)
            throw new ArgumentException("Raio deve ser maior que zero.");

        Id = Guid.NewGuid();
        ContratanteId = contratanteId;
        Localizacao = localizacao;
        Latitude = latitude;
        Longitude = longitude;
        RaioKm = raioKm;
        FormatoShow = formatoShow;
        Tipo = tipo;
        DataInicio = dataInicio;
        DataFim = dataFim;
        HorarioInicio = horarioInicio;
        HorarioFim = horarioFim;
        EquipamentoProprio = equipamentoProprio;
        BaseCacheHoraAte = baseCacheHoraAte;
        CriadoEm = DateTime.UtcNow;
    }
}