using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mottu.Fleet.Api.Dtos;
using Mottu.Fleet.Api.Features.Motos.Commands.CreateMoto;
using Mottu.Fleet.Api.Features.Motos.Commands.UpdateMotoStatus;
using Mottu.Fleet.Api.Features.Motos.Queries.GetAllMotos;
using Mottu.Fleet.Api.Features.Motos.Queries.GetMotoById;

namespace Mottu.Fleet.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/motos")]
[Authorize]
public class MotosController : ControllerBase
{
    private readonly IMediator _mediator;

    public MotosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var motos = await _mediator.Send(new GetAllMotosQuery());
        return Ok(motos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var moto = await _mediator.Send(new GetMotoByIdQuery(id));
        return Ok(moto);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateMotoRequest request)
    {
        var criada = await _mediator.Send(new CreateMotoCommand(request));
        return CreatedAtAction(nameof(GetById), new { id = criada.Id, version = "1.0" }, criada);
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateStatusRequest request)
    {
        var moto = await _mediator.Send(new UpdateMotoStatusCommand(id, request));
        return Ok(moto);
    }
}
