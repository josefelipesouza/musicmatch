namespace MusicMatch.Domain.Entities;

public abstract class Usuario
{
    public Guid Id { get; private set; }
    public string Email { get; private set; }
    public string Nome { get; private set; }
    public string CpfCnpj { get; private set; }
    public string? RazaoSocial { get; private set; }
    public string Celular1 { get; private set; }
    public string? Celular2 { get; private set; }
    public DateTime CriadoEm { get; private set; }

    protected Usuario()
    {
        Email = null!;
        Nome = null!;
        CpfCnpj = null!;
        Celular1 = null!;
    }

    protected Usuario(string email, string nome, string cpfCnpj, string? razaoSocial, string celular1, string? celular2 = null)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email é obrigatório.");
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome é obrigatório.");
        if (string.IsNullOrWhiteSpace(celular1))
            throw new ArgumentException("Celular 1 é obrigatório.");

        Id = Guid.NewGuid();
        Email = email;
        Nome = nome;
        CpfCnpj = cpfCnpj;
        RazaoSocial = razaoSocial;
        Celular1 = celular1;
        Celular2 = celular2;
        CriadoEm = DateTime.UtcNow;
    }

    public void AtualizarDados(string cpfCnpj, string? razaoSocial, string celular1, string? celular2)
    {
        if (string.IsNullOrWhiteSpace(celular1))
            throw new ArgumentException("Celular 1 é obrigatório.");
        CpfCnpj = cpfCnpj;
        RazaoSocial = razaoSocial;
        Celular1 = celular1;
        Celular2 = celular2;
    }
}