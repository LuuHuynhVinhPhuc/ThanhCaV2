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
            Id = Guid.NewGuid(),
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
        // 1. Load the hymn without including sections initially to have a clean slate for synchronization
        var hymn = await _context.Hymns
            .FirstOrDefaultAsync(h => h.Id == request.Id, cancellationToken);

        if (hymn == null) return;

        // 2. Update basic properties
        hymn.Title = request.Title;
        hymn.Author = request.Author;
        hymn.Season = request.Season;
        hymn.Notes = request.Notes;
        hymn.ProjectionSequence = request.ProjectionSequence;

        // 3. Load current sections from DB explicitly
        var currentSections = await _context.LyricSections
            .Where(s => s.HymnId == hymn.Id)
            .ToListAsync(cancellationToken);

        // 4. Identify sections to remove
        var requestSectionIds = request.Sections.Where(s => s.Id != Guid.Empty).Select(s => s.Id).ToList();
        var sectionsToRemove = currentSections.Where(s => !requestSectionIds.Contains(s.Id)).ToList();
        if (sectionsToRemove.Any())
        {
            _context.LyricSections.RemoveRange(sectionsToRemove);
        }

        // 5. Update existing or add new
        foreach (var s in request.Sections)
        {
            var existing = currentSections.FirstOrDefault(x => x.Id == s.Id && s.Id != Guid.Empty);
            if (existing != null)
            {
                // Update existing properties
                existing.Order = s.Order;
                existing.Type = s.Type;
                existing.Label = s.Label;
                existing.Content = s.Content;
                _context.LyricSections.Update(existing);
            }
            else
            {
                // Add new
                var newSection = new LyricSection
                {
                    Id = s.Id != Guid.Empty ? s.Id : Guid.NewGuid(),
                    HymnId = hymn.Id,
                    Order = s.Order,
                    Type = s.Type,
                    Label = s.Label,
                    Content = s.Content
                };
                await _context.LyricSections.AddAsync(newSection, cancellationToken);
            }
        }

        // 6. Save changes
        try 
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            // If it still happens, it might be the Hymn itself. 
            // We can try to reload and overwrite if necessary, but for now let's see if this fixes it.
            throw;
        }
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
