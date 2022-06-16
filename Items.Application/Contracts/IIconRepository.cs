
using Items.Domain.Entities;

namespace Items.Application.Contracts;

public interface IIconRepository : IGenericRepository<Icon>
{
    Task<Icon> GetById(Guid id, CancellationToken ct);
    Task<Icon> GetFile(Guid id, CancellationToken ct);
    Task<IEnumerable<Icon>> GetAll(CancellationToken ct);
    Task<Icon> Insert(Icon icon, CancellationToken ct);
    Task<Icon> Update(Icon icon, CancellationToken ct);
    Task<Icon> UpdateTitle(Guid id, string? title, CancellationToken ct);
    Task<Icon> UpdateFile(Guid id, string fileName, byte[] fileBinary, CancellationToken ct);
    Task<int> Delete(Guid id, CancellationToken ct);
}
