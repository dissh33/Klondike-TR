using MediatR;
using Microsoft.AspNetCore.Mvc;
using Offers.Api.Commands;
using Offers.Api.Dtos;
using Offers.Api.Helpers;
using Offers.Api.Queries.Offer;

namespace Offers.Admin.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OfferController : ControllerBase
{
    private readonly IMediator _mediator;

    public OfferController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OfferDto>> GetById([FromRoute] Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new OfferGetByIdQuery(id), ct);
        return new JsonResult(result);
    }

    [HttpPost("GetByPage")]
    public async Task<ActionResult<PaginationWrapper<OfferDto>>> GetByPage([FromBody] OfferGetByPageQuery query, CancellationToken ct)
    {
        var result = await _mediator.Send(query, ct);
        return new JsonResult(result);
    }

    [HttpPost("Construct")]
    public async Task<ActionResult<OfferDto>> Construct([FromBody] OfferConstructCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return new JsonResult(result);
    }
}