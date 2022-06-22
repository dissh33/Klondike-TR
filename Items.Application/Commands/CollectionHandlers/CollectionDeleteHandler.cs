using Items.Api.Commands;
using Items.Application.Contracts;
using MediatR;

namespace Items.Application.Commands.CollectionHandlers;

public class CollectionDeleteHandler : IRequestHandler<DeleteByIdCommand, int>
{
    private readonly IUnitOfWork _uow;

    public CollectionDeleteHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<int> Handle(DeleteByIdCommand request, CancellationToken ct)
    {
        var result = await _uow.CollectionRepository!.Delete(request.Id, ct);

        _uow.Commit();

        return result;
    }
}
