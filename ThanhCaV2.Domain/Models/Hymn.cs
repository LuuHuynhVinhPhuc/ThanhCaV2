using ThanhCaV2.Domain.Abstractions;
using ThanhCaV2.Share.Enums;

namespace ThanhCaV2.Domain.Models;

public class Hymn : AuditableEntity<Guid>
{
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public HymnSeason Season { get; set; }
    public string? Notes { get; set; }
    public string? ProjectionSequence { get; set; }
    
    public virtual ICollection<LyricSection> Sections { get; set; } = new List<LyricSection>();
}
