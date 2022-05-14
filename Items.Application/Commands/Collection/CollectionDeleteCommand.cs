using ItemManagementService.Api.Commands;
using ItemManagementService.Application.Contracts;
using MediatR;

namespace ItemManagementService.Application.Commands.Collection;

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
