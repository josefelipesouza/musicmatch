namespace MusicMatch.Domain.Entities;

public class Contratante : Usuario
{
    public ICollection<Evento> Eventos { get; private set; }

    private Contratante()
    {
        Eventos = new List<Evento>();
    }

    public Contratante(string email, string nome, string cpfCnpj, string? razaoSocial,
        string celular1, string? celular2 = null)
        : base(email, nome, cpfCnpj, razaoSocial, celular1, celular2)
    {
        Eventos = new List<Evento>();
    }
}