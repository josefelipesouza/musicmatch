using FluentAssertions;
using MusicMatch.Application.Commands.Artista;
using MusicMatch.Application.Validators;

namespace MusicMatch.Tests.Unit;

public class CadastrarArtistaValidatorTests
{
    private readonly CadastrarArtistaValidator _validator = new();

    [Fact]
    public void Validar_CommandValido_DevePassar()
    {
        var command = new CadastrarArtistaCommand(
            Email: "joao@email.com",
            Nome: "João Silva",
            CpfCnpj: "123.456.789-00",
            RazaoSocial: null,
            Celular1: "(11) 99999-9999",
            Celular2: null
        );

        var result = _validator.Validate(command);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validar_EmailInvalido_DeveRetornarErro()
    {
        var command = new CadastrarArtistaCommand(
            Email: "email-invalido",
            Nome: "João Silva",
            CpfCnpj: "123.456.789-00",
            RazaoSocial: null,
            Celular1: "(11) 99999-9999",
            Celular2: null
        );

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Email");
    }
}
