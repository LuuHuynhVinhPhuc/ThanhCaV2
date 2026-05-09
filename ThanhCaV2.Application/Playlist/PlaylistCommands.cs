using MediatR;
using Microsoft.EntityFrameworkCore;
using ThanhCaV2.Application.Commons;
using ThanhCaV2.Domain.Models;

namespace ThanhCaV2.Application.Playlist;

public record AddToPlaylistCommand(Guid HymnId) : IRequest<bool>;
public record RemoveFromPlaylistCommand(Guid Id) : IRequest;
public record ClearPlaylistCommand() : IRequest;

public class PlaylistCommandHandlers : 
    IRequestHandler<AddToPlaylistCommand, bool>,
    IRequestHandler<RemoveFromPlaylistCommand>,
    IRequestHandler<ClearPlaylistCommand>
{
    private readonly IApplicationDbContext _context;

    public PlaylistCommandHandlers(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(AddToPlaylistCommand request, CancellationToken cancellationToken)
    {
        // Check if hymn is already in the playlist
        var exists = await _context.MassPlaylist.AnyAsync(p => p.HymnId == request.HymnId, cancellationToken);
        if (exists) return false;

        var maxOrder = await _context.MassPlaylist.AnyAsync(cancellationToken) 
            ? await _context.MassPlaylist.MaxAsync(p => p.Order, cancellationToken) 
            : 0;

        var massHymn = new MassHymn
        {
            HymnId = request.HymnId,
            Order = maxOrder + 1
        };

        _context.MassPlaylist.Add(massHymn);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task Handle(RemoveFromPlaylistCommand request, CancellationToken cancellationToken)
    {
        var massHymn = await _context.MassPlaylist.FindAsync(new object[] { request.Id }, cancellationToken);
        if (massHymn != null)
        {
            _context.MassPlaylist.Remove(massHymn);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task Handle(ClearPlaylistCommand request, CancellationToken cancellationToken)
    {
        var items = await _context.MassPlaylist.ToListAsync(cancellationToken);
        _context.MassPlaylist.RemoveRange(items);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
