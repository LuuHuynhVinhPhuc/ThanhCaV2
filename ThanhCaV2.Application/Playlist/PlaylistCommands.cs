using MediatR;
using Microsoft.EntityFrameworkCore;
using ThanhCaV2.Application.Commons;
using ThanhCaV2.Domain.Models;

namespace ThanhCaV2.Application.Playlist;

public record AddToPlaylistCommand(Guid HymnId) : IRequest;
public record RemoveFromPlaylistCommand(Guid Id) : IRequest;
public record ClearPlaylistCommand() : IRequest;

public class PlaylistCommandHandlers : 
    IRequestHandler<AddToPlaylistCommand>,
    IRequestHandler<RemoveFromPlaylistCommand>,
    IRequestHandler<ClearPlaylistCommand>
{
    private readonly IApplicationDbContext _context;

    public PlaylistCommandHandlers(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(AddToPlaylistCommand request, CancellationToken cancellationToken)
    {
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
