using FluentValidation;
using MusicMatch.Application.Commands.Artista;

namespace MusicMatch.Application.Validators;

public class CadastrarArtistaValidator : AbstractValidator<CadastrarArtistaCommand>
{
    public CadastrarArtistaValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email é obrigatório.")
            .EmailAddress().WithMessage("Email inválido.");

        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Nome é obrigatório.")
            .MaximumLength(100).WithMessage("Nome deve ter no máximo 100 caracteres.");

        RuleFor(x => x.CpfCnpj)
            .NotEmpty().WithMessage("CPF/CNPJ é obrigatório.");

        RuleFor(x => x.Celular1)
            .NotEmpty().WithMessage("Celular 1 é obrigatório.")
            .MaximumLength(20).WithMessage("Celular deve ter no máximo 20 caracteres.");

        RuleFor(x => x.Celular2)
            .MaximumLength(20).WithMessage("Celular 2 deve ter no máximo 20 caracteres.")
            .When(x => x.Celular2 is not null);
    }
}