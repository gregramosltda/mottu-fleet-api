using FluentAssertions;
using Mottu.Fleet.Api.Dtos;
using Mottu.Fleet.Api.Validators;

namespace Mottu.Fleet.Tests.Unit.Validators;

public class CreateMotoValidatorTests
{
    private readonly CreateMotoValidator _validator = new();

    [Fact]
    public void Deve_Ser_Valido_Quando_Dados_Corretos()
    {
        var request = new CreateMotoRequest
        {
            Placa = "ABC1D23",
            Modelo = "Honda CG 160",
            Status = "disponivel",
            Quilometragem = 0,
            Bateria = 100
        };

        var result = _validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Deve_Ser_Invalido_Quando_Placa_Vazia()
    {
        var request = new CreateMotoRequest
        {
            Placa = "",
            Modelo = "Honda",
            Status = "disponivel",
            Quilometragem = 0,
            Bateria = 100
        };

        var result = _validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("Placa"));
    }

    [Fact]
    public void Deve_Ser_Invalido_Quando_Status_Invalido()
    {
        var request = new CreateMotoRequest
        {
            Placa = "ABC1D23",
            Modelo = "Honda",
            Status = "voadora",
            Quilometragem = 0,
            Bateria = 100
        };

        var result = _validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("Status"));
    }

    [Fact]
    public void Deve_Ser_Invalido_Quando_Bateria_Fora_Do_Range()
    {
        var request = new CreateMotoRequest
        {
            Placa = "ABC1D23",
            Modelo = "Honda",
            Status = "disponivel",
            Quilometragem = 0,
            Bateria = 150
        };

        var result = _validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("Bateria"));
    }
}
