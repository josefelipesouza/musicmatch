namespace MusicMatch.Domain.Entities;

using MusicMatch.Domain.Enums;

public class Agenda
{
    public Guid Id { get; private set; }
    public Guid ArtistaId { get; private set; }
    public FormatoShow FormatoShow { get; private set; }
    public bool Disponivel { get; private set; }
    public bool EquipamentoProprio { get; private set; }
    public DateTime Data { get; private set; }
    public TimeSpan HorarioInicial { get; private set; }
    public TimeSpan HorarioFinal { get; private set; }
    public decimal BaseCacheHora { get; private set; }
    public Artista? Artista { get; private set; }

    // Localização
    public string Cidade { get; private set; } = string.Empty;
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }

    private Agenda() { }

    public Agenda(
        Guid artistaId,
        FormatoShow formatoShow,
        bool equipamentoProprio,
        DateTime data,
        TimeSpan horarioInicial,
        TimeSpan horarioFinal,
        decimal baseCacheHora,
        string cidade,
        double latitude,
        double longitude)
    {
        Id = Guid.NewGuid();
        ArtistaId = artistaId;
        FormatoShow = formatoShow;
        EquipamentoProprio = equipamentoProprio;
        Disponivel = true;
        Data = data;
        HorarioInicial = horarioInicial;
        HorarioFinal = horarioFinal;
        BaseCacheHora = baseCacheHora;
        Cidade = cidade;
        Latitude = latitude;
        Longitude = longitude;
    }

    public void Cancelar()
    {
        if (Data < DateTime.UtcNow.Date ||
           (Data == DateTime.UtcNow.Date && HorarioInicial <= DateTime.UtcNow.TimeOfDay))
            throw new InvalidOperationException("Não é possível cancelar um agendamento que já ocorreu.");

        Disponivel = false;
    }
}