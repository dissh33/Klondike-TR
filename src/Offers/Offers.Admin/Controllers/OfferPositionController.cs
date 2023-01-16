using MediatR;
using Microsoft.AspNetCore.Mvc;
using Offers.Api.Dtos;
using Offers.Api.Queries.OfferItem;
using Offers.Api.Queries.OfferPosition;

namespace Offers.Admin.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OfferPositionController : ControllerBase
{
    private readonly IMediator _mediator;

    public OfferPositionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OfferPositionDto>> GetById([FromRoute] Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new OfferPositionGetByIdQuery(id), ct);
        return new JsonResult(result);
    }

    [HttpGet("GetByOffer")]
    public async Task<ActionResult<IEnumerable<OfferPositionDto>>> GetByOffer([FromQuery] Guid offerId, CancellationToken ct)
    {
        var result = await _mediator.Send(new OfferPositionGetByOfferQuery(offerId), ct);
        return new JsonResult(result);
    }
}
