using MediatR;
using Microsoft.AspNetCore.Mvc;
using MusicMatch.Application.Commands.Evento;
using MusicMatch.Application.Queries.Evento;

namespace MusicMatch.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventosController : ControllerBase
{
    private readonly IMediator _mediator;

    public EventosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarEventoCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetByContratante), new { contratanteId = result.ContratanteId }, result);
    }

    [HttpGet("contratante/{contratanteId}")]
    public async Task<IActionResult> GetByContratante(Guid contratanteId)
    {
        var result = await _mediator.Send(new GetEventosByContratanteQuery(contratanteId));
        return Ok(result);
    }

    [HttpDelete("{eventoId}")]
    public async Task<IActionResult> CancelarEvento(Guid eventoId)
    {
        var result = await _mediator.Send(new CancelarEventoCommand(eventoId));
        if (!result) return NotFound();
        return NoContent();
    }
}