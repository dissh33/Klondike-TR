using Items.Api.Commands;
using Items.Application.Contracts;
using MediatR;

namespace Items.Application.Commands.Material;

public class MaterialDeleteHandler : IRequestHandler<DeleteByIdCommand, int>
{
    private readonly IUnitOfWork _uow;

    public MaterialDeleteHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<int> Handle(DeleteByIdCommand request, CancellationToken ct)
    {
        var result = await _uow.MaterialRepository!.Delete(request.Id, ct);

        _uow.Commit();

        return result;
    }
}
