using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mottu.Fleet.Api.Features.Motos.Queries.GetAllMotos;

namespace Mottu.Fleet.Api.Controllers;

[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/motos")]
[Authorize]
public class MotosV2Controller : ControllerBase
{
    private readonly IMediator _mediator;

    public MotosV2Controller(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int size = 10)
    {
        var motos = await _mediator.Send(new GetAllMotosQuery());

        var total = motos.Count();
        var paginado = motos
            .Skip((page - 1) * size)
            .Take(size)
            .ToList();

        return Ok(new
        {
            page,
            size,
            total,
            totalPages = (int)Math.Ceiling((double)total / size),
            data = paginado
        });
    }
}
