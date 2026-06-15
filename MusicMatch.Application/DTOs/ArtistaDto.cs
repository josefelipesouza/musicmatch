namespace MusicMatch.Application.DTOs;

public class ArtistaDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string? CpfCnpj { get; set; }
    public string? RazaoSocial { get; set; }
    public string Celular1 { get; set; } = string.Empty;
    public string? Celular2 { get; set; }
}