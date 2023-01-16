using Items.Api.Commands;
using Items.Api.Commands.Collection;
using Items.Api.Dtos.Collection;
using Items.Api.Dtos.CollectionItem;
using Items.Api.Queries.Collection;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Items.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollectionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CollectionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CollectionDto>>> GetAll(CancellationToken ct)
        {
            var result = await _mediator.Send(new CollectionGetAllQuery(), ct);
            return new JsonResult(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CollectionDto>> GetById(Guid id, CancellationToken ct)
        {
            var result = await _mediator.Send(new CollectionGetByIdQuery(id), ct);
            return new JsonResult(result);
        }

        [HttpGet("GetFull/{id}")]
        public async Task<ActionResult<CollectionFullDto>> GetFull(Guid id, CancellationToken ct)
        {
            var result = await _mediator.Send(new CollectionGetFullQuery(id), ct);
            return new JsonResult(result);
        }

        [HttpGet("GetByFilter")]
        public async Task<ActionResult<IEnumerable<CollectionDto>>> GetByFilter([FromQuery] CollectionGetByFilterQuery request, CancellationToken ct)
        {
            var result = await _mediator.Send(request, ct);
            return new JsonResult(result);
        }

        [HttpPost]
        public async Task<ActionResult<CollectionDto>> Create([FromForm] CollectionAddCommand request, CancellationToken ct)
        {
            var result = await _mediator.Send(request, ct);
            return new JsonResult(result);
        }

        [HttpPost("Construct")]
        public async Task<ActionResult<CollectionFullDto>> Construct(
            [FromForm] string jsonInput,
            [FromForm] IEnumerable<IFormFile> files, 
            CancellationToken ct)
        {
            var request = JsonConvert.DeserializeObject<CollectionConstructCommand>(jsonInput);
            var fileList = files.ToList();

            var collectionFileStream = new MemoryStream();
            await fileList.First().CopyToAsync(collectionFileStream, ct);
            var collectionBinary = collectionFileStream.GetBuffer();

            request!.Icon.FileBinary = collectionBinary;
            request.Icon.FileName = fileList.First().FileName;

            foreach (var icon in fileList.Skip(1))
            {
                var fileStream = new MemoryStream();
                await icon.CopyToAsync(fileStream, ct);
                var binary = fileStream.GetBuffer();

                var current = request.Items.FirstOrDefault(itemAddDto => itemAddDto.Icon.FileBinary == null) ?? new CollectionItemAddDto();
                current.Icon.FileBinary = binary;
                current.Icon.FileName = icon.FileName;
            }

            var result = await _mediator.Send(request, ct);
            return new JsonResult(result);
        }

        [HttpPut("")]
        public async Task<ActionResult<CollectionDto>> Update([FromForm] CollectionUpdateCommand request, CancellationToken ct)
        {
            var result = await _mediator.Send(request, ct);
            return new JsonResult(result);
        }

        [HttpPut("UpdateName")]
        public async Task<ActionResult<CollectionDto>> UpdateName([FromForm] CollectionUpdateNameCommand request, CancellationToken ct)
        {
            var result = await _mediator.Send(request, ct);
            return new JsonResult(result);
        }

        [HttpPut("UpdateIcon")]
        public async Task<ActionResult<CollectionDto>> UpdateIcon([FromForm] CollectionUpdateIconCommand request, CancellationToken ct)
        {
            var result = await _mediator.Send(request, ct);
            return new JsonResult(result);
        }

        [HttpPut("UpdateStatus")]
        public async Task<ActionResult<CollectionDto>> UpdateStatus([FromForm] CollectionUpdateStatusCommand request, CancellationToken ct)
        {
            var result = await _mediator.Send(request, ct);
            return new JsonResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> Delete(Guid id, CancellationToken ct)
        {
            var result = await _mediator.Send(new DeleteByIdCommand(id), ct);
            return new JsonResult(result);
        }
    }
}
