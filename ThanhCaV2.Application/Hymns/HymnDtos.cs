using ThanhCaV2.Share.Enums;

namespace ThanhCaV2.Application.Hymns;

public class HymnDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public HymnSeason Season { get; set; }
    public string? Notes { get; set; }
    public string? ProjectionSequence { get; set; }
}

public class LyricSectionDto
{
    public Guid Id { get; set; }
    public int Order { get; set; }
    public LyricType Type { get; set; }
    public string? Label { get; set; }
    public string Content { get; set; } = string.Empty;
}

public class HymnDetailDto : HymnDto
{
    public List<LyricSectionDto> Sections { get; set; } = new();
}

public class PlaylistDto
{
    public Guid Id { get; set; }
    public Guid HymnId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int Order { get; set; }
}

