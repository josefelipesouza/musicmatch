namespace MusicMatch.Domain.Entities;

public class Mensagem
{
    public Guid Id { get; private set; }
    public Guid AutorId { get; private set; }
    public Guid DestinoId { get; private set; }
    public string Msg { get; private set; }
    public DateTime DataHora { get; private set; }

    private Mensagem() 
    { 
        Msg = null!;
    }

    public Mensagem(Guid autorId, Guid destinoId, string msg)
    {
        if (string.IsNullOrWhiteSpace(msg))
            throw new ArgumentException("Mensagem não pode ser vazia.");// fazer tratativa na camada de entrada

        Id = Guid.NewGuid();
        AutorId = autorId;
        DestinoId = destinoId;
        Msg = msg;
        DataHora = DateTime.UtcNow;
    }
}