using FluentAssertions;
using Moq;
using Mottu.Fleet.Api.Dtos;
using Mottu.Fleet.Api.Features.Motos.Commands.UpdateMotoStatus;
using Mottu.Fleet.Api.Models;
using Mottu.Fleet.Api.Repositories;
using Mottu.Fleet.Api.Validators;

namespace Mottu.Fleet.Tests.Unit.Handlers;

public class UpdateMotoStatusHandlerTests
{
    [Fact]
    public async Task Deve_Atualizar_Status_Quando_Dados_Validos()
    {
        var moto = new Moto { Id = 1, Placa = "ABC1D23", Modelo = "Honda", Status = "disponivel" };

        var repoMock = new Mock<IMotoRepository>();
        repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(moto);
        repoMock.Setup(r => r.UpdateAsync(It.IsAny<Moto>()))
            .ReturnsAsync((Moto m) => m);

        var validator = new UpdateStatusValidator();
        var handler = new UpdateMotoStatusHandler(repoMock.Object, validator);

        var result = await handler.Handle(
            new UpdateMotoStatusCommand(1, new UpdateStatusRequest { Status = "manutencao" }),
            CancellationToken.None);

        result.Status.Should().Be("manutencao");
    }

    [Fact]
    public async Task Deve_Lancar_NotFoundException_Quando_Moto_Nao_Existe()
    {
        var repoMock = new Mock<IMotoRepository>();
        repoMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Moto?)null);

        var validator = new UpdateStatusValidator();
        var handler = new UpdateMotoStatusHandler(repoMock.Object, validator);

        Func<Task> act = async () => await handler.Handle(
            new UpdateMotoStatusCommand(999, new UpdateStatusRequest { Status = "manutencao" }),
            CancellationToken.None);

        await act.Should().ThrowAsync<Mottu.Fleet.Api.Exceptions.NotFoundException>();
    }
}
