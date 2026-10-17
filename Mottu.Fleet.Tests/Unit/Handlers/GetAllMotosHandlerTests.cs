using FluentAssertions;
using Moq;
using Mottu.Fleet.Api.Features.Motos.Queries.GetAllMotos;
using Mottu.Fleet.Api.Models;
using Mottu.Fleet.Api.Repositories;

namespace Mottu.Fleet.Tests.Unit.Handlers;

public class GetAllMotosHandlerTests
{
    [Fact]
    public async Task Deve_Retornar_Lista_De_Motos()
    {
        var motos = new List<Moto>
        {
            new() { Id = 1, Placa = "ABC1D23", Modelo = "Honda", Status = "disponivel" },
            new() { Id = 2, Placa = "XYZ9A87", Modelo = "Yamaha", Status = "alugada" }
        };

        var repoMock = new Mock<IMotoRepository>();
        repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(motos);

        var handler = new GetAllMotosHandler(repoMock.Object);
        var result = await handler.Handle(new GetAllMotosQuery(), CancellationToken.None);

        result.Should().HaveCount(2);
        result.Should().Contain(m => m.Placa == "ABC1D23");
    }
}
