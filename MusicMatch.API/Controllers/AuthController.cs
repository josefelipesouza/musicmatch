using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using MusicMatch.Authentication;
using MusicMatch.Domain.Interfaces;

namespace MusicMatch.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IArtistaRepository _artistaRepository;
    private readonly IContratanteRepository _contratanteRepository;
    private readonly JwtService _jwtService;

    public AuthController(
        IArtistaRepository artistaRepository,
        IContratanteRepository contratanteRepository,
        JwtService jwtService)
    {
        _artistaRepository = artistaRepository;
        _contratanteRepository = contratanteRepository;
        _jwtService = jwtService;
    }

    [HttpGet("google")]
    public IActionResult LoginGoogle()
    {
        var properties = new AuthenticationProperties
        {
            RedirectUri = Url.Action(nameof(GoogleCallback))
        };
        properties.Items["prompt"] = "select_account";
        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }

    [HttpGet("google/callback")]
    public async Task<IActionResult> GoogleCallback()
    {
        var result = await HttpContext.AuthenticateAsync("Cookies");

        if (!result.Succeeded)
            return Redirect("http://localhost:5173/?erro=auth");

        var email = result.Principal?.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
        var nome = result.Principal?.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;

        if (string.IsNullOrEmpty(email))
            return Redirect("http://localhost:5173/?erro=email");

        // Verifica se é artista ou contratante
        var artista = await _artistaRepository.GetByEmailAsync(email);
        if (artista is not null)
        {
            var token = _jwtService.GenerateToken(artista.Id, artista.Email, artista.Nome, "Artista");
            return Redirect($"http://localhost:5173/auth/callback?token={token}&tipo=Artista");
        }

        var contratante = await _contratanteRepository.GetByEmailAsync(email);
        if (contratante is not null)
        {
            var token = _jwtService.GenerateToken(contratante.Id, contratante.Email, contratante.Nome, "Contratante");
            return Redirect($"http://localhost:5173/auth/callback?token={token}&tipo=Contratante");
        }

        // Primeiro acesso
        return Redirect($"http://localhost:5173/auth/callback?primeiroAcesso=true&email={Uri.EscapeDataString(email!)}&nome={Uri.EscapeDataString(nome ?? "")}");
    }
}