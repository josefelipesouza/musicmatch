using FluentValidation;
using MusicMatch.Application.Commands.Evento;

namespace MusicMatch.Application.Validators;

public class CriarEventoValidator : AbstractValidator<CriarEventoCommand>
{
    public CriarEventoValidator()
    {
        RuleFor(x => x.Localizacao)
            .NotEmpty().WithMessage("Localização é obrigatória.");

        RuleFor(x => x.DataInicio)
            .GreaterThan(DateTime.UtcNow).WithMessage("Data início deve ser no futuro.");

        RuleFor(x => x.DataFim)
            .GreaterThanOrEqualTo(x => x.DataInicio).WithMessage("Data fim deve ser após a data início.");

        RuleFor(x => x.BaseCacheHoraAte)
            .GreaterThan(0).WithMessage("Cache hora deve ser maior que zero.");

        RuleFor(x => x.ContratanteId)
            .NotEmpty().WithMessage("ContratanteId é obrigatório.");
    }
}