using Microsoft.EntityFrameworkCore;
using ThanhCaV2.Domain.Models;

namespace ThanhCaV2.Application.Commons;

public interface IApplicationDbContext
{
    DbSet<Hymn> Hymns { get; }
    DbSet<LyricSection> LyricSections { get; }
    DbSet<MassHymn> MassPlaylist { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
