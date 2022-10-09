using Items.Api.Commands;
using Items.Application.Contracts;
using MediatR;

namespace Items.Application.CommandHandlers.IconHandlers;

public class IconDeleteHandler : IRequestHandler<DeleteByIdCommand, int>
{
    private readonly IUnitOfWork _uow;

    public IconDeleteHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<int> Handle(DeleteByIdCommand request, CancellationToken ct)
    {
        var result = await _uow.IconRepository.Delete(request.Id, ct);

        _uow.Commit();

        return result;
    }
}
