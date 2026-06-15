using MediatR;
using Microsoft.AspNetCore.Mvc;
using MusicMatch.Application.Commands.Artista;
using MusicMatch.Application.Queries.Artista;
using MusicMatch.Domain.Enums;

namespace MusicMatch.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArtistasController : ControllerBase
{
    private readonly IMediator _mediator;

    public ArtistasController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Cadastrar([FromBody] CadastrarArtistaCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizarArtistaCommand command)
    {
        if (id != command.Id) return BadRequest();
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetArtistaByIdQuery(id));
        if (result is null) return NotFound();
        return Ok(result);
    }

    [HttpGet("buscar")]
    public async Task<IActionResult> Buscar(
        [FromQuery] FormatoShow formatoShow,
        [FromQuery] double latitude,
        [FromQuery] double longitude,
        [FromQuery] double raioKm,
        [FromQuery] DateTime data,
        [FromQuery] TimeSpan horarioInicio,
        [FromQuery] TimeSpan horarioFim,
        [FromQuery] bool? equipamentoProprio,
        [FromQuery] decimal? baseCacheHoraAte)
    {
        var dataUtc = DateTime.SpecifyKind(data, DateTimeKind.Utc);

        var result = await _mediator.Send(new BuscarArtistasQuery(
            formatoShow, latitude, longitude, raioKm,
            dataUtc, horarioInicio, horarioFim,
            equipamentoProprio, baseCacheHoraAte));

        return Ok(result);
    }

    [HttpPost("agenda")]
    public async Task<IActionResult> CadastrarAgenda([FromBody] CadastrarAgendaCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet("{id}/agendas")]
    public async Task<IActionResult> GetAgendas(Guid id)
    {
        var result = await _mediator.Send(new GetAgendasArtistaQuery(id));
        return Ok(result);
    }

    [HttpDelete("agenda/{agendaId}")]
    public async Task<IActionResult> CancelarAgenda(Guid agendaId)
    {
        var result = await _mediator.Send(new CancelarAgendaCommand(agendaId));
        if (!result) return NotFound();
        return NoContent();
    }

}