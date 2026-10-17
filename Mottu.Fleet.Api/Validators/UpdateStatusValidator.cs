using FluentValidation;
using Mottu.Fleet.Api.Dtos;

namespace Mottu.Fleet.Api.Validators;

public class UpdateStatusValidator : AbstractValidator<UpdateStatusRequest>
{
    private static readonly string[] StatusValidos =
        { "disponivel", "alugada", "manutencao", "inativa" };

    public UpdateStatusValidator()
    {
        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status é obrigatório")
            .Must(s => StatusValidos.Contains(s))
            .WithMessage($"Status inválido. Use: {string.Join(", ", StatusValidos)}");
    }
}