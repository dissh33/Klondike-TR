using Items.Api.Commands;
using Items.Application.Contracts;
using MediatR;

namespace Items.Application.Commands.CollectionItemHandlers;

public class CollectionItemDeleteHandler : IRequestHandler<DeleteByIdCommand, int>
{
    private readonly IUnitOfWork _uow;

    public CollectionItemDeleteHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<int> Handle(DeleteByIdCommand request, CancellationToken ct)
    {
        var result = await _uow.CollectionItemRepository!.Delete(request.Id, ct);

        _uow.Commit();

        return result;
    }
}