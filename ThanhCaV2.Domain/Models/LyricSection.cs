using ThanhCaV2.Domain.Abstractions;
using ThanhCaV2.Share.Enums;

namespace ThanhCaV2.Domain.Models;

public class LyricSection : BaseEntity<Guid>
{
    public Guid HymnId { get; set; }
    public int Order { get; set; }
    public LyricType Type { get; set; }
    public string? Label { get; set; }
    public string Content { get; set; } = string.Empty;

    public virtual Hymn Hymn { get; set; } = null!;
}
