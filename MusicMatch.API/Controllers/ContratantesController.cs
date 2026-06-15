using MediatR;
using Microsoft.AspNetCore.Mvc;
using MusicMatch.Application.Queries.Contratante;
using MusicMatch.Application.Commands.Contratante;
using MusicMatch.Application.Commands.Evento;
using MusicMatch.Application.Queries.Evento;

namespace MusicMatch.API.Controllers;

[ApiController]
[Route("api/[controller]")] 
public class ContratantesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ContratantesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Cadastrar([FromBody] CadastrarContratanteCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizarContratanteCommand command)
    {
        if (id != command.Id) return BadRequest();
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetContratanteByIdQuery(id));
        if (result is null) return NotFound();
        return Ok(result);
    }

    // ==== MÉTODOS DE EVENTOS  ====

    [HttpPost("evento")] 
    public async Task<IActionResult> Criar([FromBody] CriarEventoCommand command)
    {
        var result = await _mediator.Send(command);
        // Ajustado para referenciar a rota de listagem abaixo
        return CreatedAtAction(nameof(GetEventos), new { id = result.ContratanteId }, result);
    }

    [HttpGet("{id}/eventos")] 
    public async Task<IActionResult> GetEventos(Guid id)
    {
        var result = await _mediator.Send(new GetEventosByContratanteQuery(id));
        return Ok(result);
    }

    // ==== MÉTODOS RabbitMQ  ====

    [HttpPost("notificar-artista")]
    public async Task<IActionResult> NotificarArtista([FromBody] NotificarArtistaCommand command)
    {
        var result = await _mediator.Send(command);
        if (!result) return NotFound();
        return Ok(new { message = "Notificação enviada com sucesso." });
    }

}