using FluentAssertions;
using Moq;
using Mottu.Fleet.Api.Features.Motos.Queries.GetMotoById;
using Mottu.Fleet.Api.Models;
using Mottu.Fleet.Api.Repositories;

namespace Mottu.Fleet.Tests.Unit.Handlers;

public class GetMotoByIdHandlerTests
{
    [Fact]
    public async Task Deve_Retornar_Moto_Quando_Existe()
    {
        var moto = new Moto { Id = 1, Placa = "ABC1D23", Modelo = "Honda", Status = "disponivel" };

        var repoMock = new Mock<IMotoRepository>();
        repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(moto);

        var handler = new GetMotoByIdHandler(repoMock.Object);
        var result = await handler.Handle(new GetMotoByIdQuery(1), CancellationToken.None);

        result.Should().NotBeNull();
        result.Id.Should().Be(1);
    }

    [Fact]
    public async Task Deve_Lancar_NotFoundException_Quando_Nao_Existe()
    {
        var repoMock = new Mock<IMotoRepository>();
        repoMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Moto?)null);

        var handler = new GetMotoByIdHandler(repoMock.Object);

        Func<Task> act = async () => await handler.Handle(new GetMotoByIdQuery(999), CancellationToken.None);

        await act.Should().ThrowAsync<Mottu.Fleet.Api.Exceptions.NotFoundException>();
    }
}
