using Items.Api.Commands;
using Items.Api.Commands.Collection;
using Items.Api.Dtos;
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
        public async Task<ActionResult<IEnumerable<CollectionDto>>> Get(CancellationToken ct)
        {
            var result = await _mediator.Send(new CollectionGetAllQuery(), ct);
            return new JsonResult(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CollectionDto>> Get(Guid id, CancellationToken ct)
        {
            var result = await _mediator.Send(new CollectionGetByIdQuery(id), ct);
            return new JsonResult(result);
        }

        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<CollectionDto>>> GetByFilter([FromQuery] CollectionGetByFilterQuery request, CancellationToken ct)
        {
            var result = await _mediator.Send(request, ct);
            return new JsonResult(result);
        }

        [HttpPost]
        public async Task<ActionResult<CollectionDto>> Post([FromForm] CollectionAddCommand request, CancellationToken ct)
        {
            var result = await _mediator.Send(request, ct);
            return new JsonResult(result);
        }

        [HttpPost("construct")]
        public async Task<ActionResult<CollectionDto>> Construct(
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

        [HttpPut("update")]
        public async Task<ActionResult<CollectionDto>> Put([FromForm] CollectionUpdateCommand request, CancellationToken ct)
        {
            var result = await _mediator.Send(request, ct);
            return new JsonResult(result);
        }

        [HttpPut("update/name")]
        public async Task<ActionResult<CollectionDto>> UpdateName([FromForm] CollectionUpdateNameCommand request, CancellationToken ct)
        {
            var result = await _mediator.Send(request, ct);
            return new JsonResult(result);
        }

        [HttpPut("update/icon")]
        public async Task<ActionResult<CollectionDto>> UpdateIcon([FromForm] CollectionUpdateIconCommand request, CancellationToken ct)
        {
            var result = await _mediator.Send(request, ct);
            return new JsonResult(result);
        }

        [HttpPut("update/status")]
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
