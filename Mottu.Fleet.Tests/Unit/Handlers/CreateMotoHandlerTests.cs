using FluentAssertions;
using Moq;
using Mottu.Fleet.Api.Dtos;
using Mottu.Fleet.Api.Features.Motos.Commands.CreateMoto;
using Mottu.Fleet.Api.Models;
using Mottu.Fleet.Api.Repositories;
using Mottu.Fleet.Api.Validators;

namespace Mottu.Fleet.Tests.Unit.Handlers;

public class CreateMotoHandlerTests
{
    [Fact]
    public async Task Deve_Criar_Moto_Quando_Dados_Validos()
    {
        var request = new CreateMotoRequest
        {
            Placa = "ABC1D23",
            Modelo = "Honda",
            Status = "disponivel",
            Quilometragem = 0,
            Bateria = 100
        };

        var repoMock = new Mock<IMotoRepository>();
        repoMock.Setup(r => r.AddAsync(It.IsAny<Moto>()))
            .ReturnsAsync((Moto m) => { m.Id = 1; return m; });

        var validator = new CreateMotoValidator();
        var handler = new CreateMotoHandler(repoMock.Object, validator);

        var result = await handler.Handle(new CreateMotoCommand(request), CancellationToken.None);

        result.Should().NotBeNull();
        result.Placa.Should().Be("ABC1D23");
        result.Id.Should().Be(1);
    }

    [Fact]
    public async Task Deve_Lancar_ValidationException_Quando_Dados_Invalidos()
    {
        var request = new CreateMotoRequest
        {
            Placa = "",
            Modelo = "Honda",
            Status = "disponivel",
            Quilometragem = 0,
            Bateria = 100
        };

        var repoMock = new Mock<IMotoRepository>();
        var validator = new CreateMotoValidator();
        var handler = new CreateMotoHandler(repoMock.Object, validator);

        Func<Task> act = async () => await handler.Handle(new CreateMotoCommand(request), CancellationToken.None);

        await act.Should().ThrowAsync<Mottu.Fleet.Api.Exceptions.ValidationException>();
    }
}
