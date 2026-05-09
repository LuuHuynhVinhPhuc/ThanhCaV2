using ThanhCaV2.Domain.Abstractions;

namespace ThanhCaV2.Domain.Models;

public class MassHymn : BaseEntity<Guid>
{
    public Guid HymnId { get; set; }
    public int Order { get; set; }

    public virtual Hymn Hymn { get; set; } = null!;
}
