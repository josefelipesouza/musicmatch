using FluentAssertions;
using MusicMatch.Domain.Services;

namespace MusicMatch.Tests.Unit;

public class GeoCalculatorTests
{
    [Fact]
    public void CalcularDistanciaKm_MesmoPonto_DeveRetornarZero()
    {
        double lat = -23.5505;
        double lon = -46.6333;

        var distancia = GeoCalculator.CalcularDistanciaKm(lat, lon, lat, lon);

        distancia.Should().BeApproximately(0, precision: 0.001);
    }

    [Fact]
    public void CalcularDistanciaKm_SaoPauloParaRioDeJaneiro_DeveRetornarAproximadamente357Km()
    {
        double latSP = -23.5505;
        double lonSP = -46.6333;
        double latRJ = -22.9068;
        double lonRJ = -43.1729;

        var distancia = GeoCalculator.CalcularDistanciaKm(latSP, lonSP, latRJ, lonRJ);

        distancia.Should().BeInRange(350, 365);
    }
}
