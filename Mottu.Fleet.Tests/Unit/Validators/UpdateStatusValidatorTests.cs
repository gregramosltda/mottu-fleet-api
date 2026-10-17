using FluentAssertions;
using Mottu.Fleet.Api.Dtos;
using Mottu.Fleet.Api.Validators;

namespace Mottu.Fleet.Tests.Unit.Validators;

public class UpdateStatusValidatorTests
{
    private readonly UpdateStatusValidator _validator = new();

    [Theory]
    [InlineData("disponivel")]
    [InlineData("alugada")]
    [InlineData("manutencao")]
    [InlineData("inativa")]
    public void Deve_Ser_Valido_Para_Status_Permitidos(string status)
    {
        var request = new UpdateStatusRequest { Status = status };

        var result = _validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("voadora")]
    [InlineData("")]
    [InlineData("status_invalido")]
    public void Deve_Ser_Invalido_Para_Status_Nao_Permitidos(string status)
    {
        var request = new UpdateStatusRequest { Status = status };

        var result = _validator.Validate(request);

        result.IsValid.Should().BeFalse();
    }
}
