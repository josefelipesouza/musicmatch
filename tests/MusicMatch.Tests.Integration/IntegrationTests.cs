using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using MusicMatch.Application.Commands.Artista;

namespace MusicMatch.Tests.Integration;

public class ArtistasIntegrationTests : IClassFixture<MusicMatchWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ArtistasIntegrationTests(MusicMatchWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task POST_Artistas_CommandInvalido_DeveRetornar400()
    {
        var command = new CadastrarArtistaCommand(
            Email: "email-invalido",
            Nome: "",
            CpfCnpj: "",
            RazaoSocial: null,
            Celular1: "",
            Celular2: null
        );

        var response = await _client.PostAsJsonAsync("/api/artistas", command);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GET_EventosPorContratante_IdInexistente_DeveRetornar200ComListaVazia()
    {
        var contratanteId = Guid.NewGuid();

        var response = await _client.GetAsync($"/api/eventos/contratante/{contratanteId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var eventos = await response.Content.ReadFromJsonAsync<List<object>>();
        eventos.Should().BeEmpty();
    }
}
