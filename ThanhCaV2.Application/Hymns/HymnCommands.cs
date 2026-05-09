using MediatR;
using Microsoft.EntityFrameworkCore;
using ThanhCaV2.Application.Commons;
using ThanhCaV2.Domain.Models;
using ThanhCaV2.Share.Enums;

namespace ThanhCaV2.Application.Hymns;

public record CreateHymnCommand(
    string Title,
    string Author,
    HymnSeason Season,
    string? Notes,
    string? ProjectionSequence,
    List<LyricSectionDto> Sections
) : IRequest<Guid>;

public record UpdateHymnCommand(
    Guid Id,
    string Title,
    string Author,
    HymnSeason Season,
    string? Notes,
    string? ProjectionSequence,
    List<LyricSectionDto> Sections
) : IRequest;

public record DeleteHymnCommand(Guid Id) : IRequest;

public class HymnCommandHandlers : 
    IRequestHandler<CreateHymnCommand, Guid>,
    IRequestHandler<UpdateHymnCommand>,
    IRequestHandler<DeleteHymnCommand>
{
    private readonly IApplicationDbContext _context;

    public HymnCommandHandlers(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateHymnCommand request, CancellationToken cancellationToken)
    {
        var hymn = new Hymn
        {
            Title = request.Title,
            Author = request.Author,
            Season = request.Season,
            Notes = request.Notes,
            ProjectionSequence = request.ProjectionSequence,
            Sections = request.Sections.Select(s => new LyricSection
            {
                Id = s.Id != Guid.Empty ? s.Id : Guid.NewGuid(),
                Order = s.Order,
                Type = s.Type,
                Label = s.Label,
                Content = s.Content
            }).ToList()
        };

        _context.Hymns.Add(hymn);
        await _context.SaveChangesAsync(cancellationToken);

        return hymn.Id;
    }

    public async Task Handle(UpdateHymnCommand request, CancellationToken cancellationToken)
    {
        var hymn = await _context.Hymns
            .Include(h => h.Sections)
            .FirstOrDefaultAsync(h => h.Id == request.Id, cancellationToken);

        if (hymn == null) return;

        hymn.Title = request.Title;
        hymn.Author = request.Author;
        hymn.Season = request.Season;
        hymn.Notes = request.Notes;
        hymn.ProjectionSequence = request.ProjectionSequence;

        hymn.Sections.Clear();
        foreach (var s in request.Sections)
        {
            hymn.Sections.Add(new LyricSection
            {
                Id = s.Id != Guid.Empty ? s.Id : Guid.NewGuid(),
                HymnId = hymn.Id,
                Order = s.Order,
                Type = s.Type,
                Label = s.Label,
                Content = s.Content
            });
        }

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task Handle(DeleteHymnCommand request, CancellationToken cancellationToken)
    {
        var hymn = await _context.Hymns.FindAsync(new object[] { request.Id }, cancellationToken);
        if (hymn != null)
        {
            _context.Hymns.Remove(hymn);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
