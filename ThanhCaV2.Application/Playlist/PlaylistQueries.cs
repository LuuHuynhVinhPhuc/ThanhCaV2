using MediatR;
using Microsoft.EntityFrameworkCore;
using ThanhCaV2.Application.Commons;
using ThanhCaV2.Application.Hymns;

namespace ThanhCaV2.Application.Playlist;

public record GetPlaylistQuery() : IRequest<List<PlaylistDto>>;

public class GetPlaylistQueryHandler : IRequestHandler<GetPlaylistQuery, List<PlaylistDto>>
{
    private readonly IApplicationDbContext _context;

    public GetPlaylistQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<PlaylistDto>> Handle(GetPlaylistQuery request, CancellationToken cancellationToken)
    {
        return await _context.MassPlaylist
            .AsNoTracking()
            .Include(p => p.Hymn)
            .OrderBy(p => p.Order)
            .Select(p => new PlaylistDto
            {
                Id = p.Id,
                HymnId = p.HymnId,
                Title = p.Hymn.Title,
                Author = p.Hymn.Author,
                Order = p.Order
            })
            .ToListAsync(cancellationToken);
    }
}
