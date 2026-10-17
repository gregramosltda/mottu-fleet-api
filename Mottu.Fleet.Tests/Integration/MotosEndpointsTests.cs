using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Mottu.Fleet.Tests.Integration;

public class MotosEndpointsTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public MotosEndpointsTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GET_Motos_Sem_Token_Deve_Retornar_401()
    {
        var response = await _client.GetAsync("/api/motos");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task POST_Login_Com_Credenciais_Validas_Deve_Retornar_200()
    {
        var login = new { username = "admin", password = "admin123" };

        var response = await _client.PostAsJsonAsync("/api/auth/login", login);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("token");
    }

    [Fact]
    public async Task POST_Login_Com_Credenciais_Invalidas_Deve_Retornar_401()
    {
        var login = new { username = "admin", password = "errada" };

        var response = await _client.PostAsJsonAsync("/api/auth/login", login);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GET_Health_Deve_Retornar_200()
    {
        var response = await _client.GetAsync("/health");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}

