using FluentValidation;
using Mottu.Fleet.Api.Dtos;

namespace Mottu.Fleet.Api.Validators;

public class CreateMotoValidator : AbstractValidator<CreateMotoRequest>
{
    private static readonly string[] StatusValidos =
        { "disponivel", "alugada", "manutencao", "inativa" };

    public CreateMotoValidator()
    {
        RuleFor(x => x.Placa)
            .NotEmpty().WithMessage("Placa é obrigatória")
            .MaximumLength(8).WithMessage("Placa deve ter no máximo 8 caracteres");

        RuleFor(x => x.Modelo)
            .NotEmpty().WithMessage("Modelo é obrigatório")
            .MaximumLength(100).WithMessage("Modelo deve ter no máximo 100 caracteres");

        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status é obrigatório")
            .Must(s => StatusValidos.Contains(s))
            .WithMessage($"Status inválido. Use: {string.Join(", ", StatusValidos)}");

        RuleFor(x => x.Bateria)
            .InclusiveBetween(0, 100).WithMessage("Bateria deve estar entre 0 e 100");
    }
}