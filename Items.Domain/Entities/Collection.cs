using System.Runtime.CompilerServices;
using ItemManagementService.Domain.Enums;

namespace ItemManagementService.Domain.Entities;

public class Collection : BaseEntity, ITradableItem
{
    public string? Name { get; }
    public Guid IconId { get;  }
    public ItemStatus? Status { get;  }
    public DateTime DateAdded { get;  }
    

    public Collection()
    {
        DateAdded = DateTime.UtcNow;
    }

    public Collection(string? name, Guid iconId, ItemStatus status = ItemStatus.Active, Guid? id = null, string? externalId = null)
        : base (id, externalId)
    {
        Name = name;
        IconId = iconId;
        Status = status;

        DateAdded = DateTime.UtcNow;
    }
}