using MusicMatch.Domain.Enums;

namespace MusicMatch.Domain.Entities;

public class FormatoShowArtista
{
    public Guid Id { get; private set; }
    public Guid ArtistaId { get; private set; }
    public FormatoShow Tipo { get; private set; }
    public Artista Artista { get; private set; } = null!;
    public ICollection<Agenda> Agendas { get; private set; } = new List<Agenda>();

    private FormatoShowArtista() { }

    public FormatoShowArtista(Guid artistaId, FormatoShow tipo)
    {
        Id = Guid.NewGuid();
        ArtistaId = artistaId;
        Tipo = tipo;
    }
}