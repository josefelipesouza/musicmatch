namespace MusicMatch.Domain.Entities;

public class Artista : Usuario
{
    public ICollection<Agenda> Agendas { get; private set; }

    private Artista()
    {
        Agendas = new List<Agenda>();
    }

    public Artista(string email, string nome, string cpfCnpj, string? razaoSocial,
        string celular1, string? celular2 = null)
        : base(email, nome, cpfCnpj, razaoSocial, celular1, celular2)
    {
        Agendas = new List<Agenda>();
    }
}