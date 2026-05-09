using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ThanhCaV2.Application.Commons;
using ThanhCaV2.Share.Enums;

namespace ThanhCaV2.Application.Hymns;

public record GetHymnsQuery(
    string? SearchTerm = null,
    string? Author = null,
    HymnSeason? Season = null,
    int PageNumber = 1,
    int PageSize = 10
) : IRequest<PaginatedList<HymnDto>>;

public class GetHymnsQueryHandler : IRequestHandler<GetHymnsQuery, PaginatedList<HymnDto>>
{
    private readonly IApplicationDbContext _context;

    public GetHymnsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<HymnDto>> Handle(GetHymnsQuery request, CancellationToken cancellationToken)
    {
        var allHymns = await _context.Hymns
            .AsNoTracking()
            .Include(h => h.Sections)
            .OrderBy(h => h.Title)
            .ProjectToType<HymnDto>()
            .ToListAsync(cancellationToken);

        var filteredQuery = allHymns.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var search = request.SearchTerm.Trim();
            filteredQuery = filteredQuery.Where(h => h.Title.Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(request.Author))
        {
            var authorSearch = request.Author.Trim();
            filteredQuery = filteredQuery.Where(h => h.Author != null && h.Author.Contains(authorSearch, StringComparison.OrdinalIgnoreCase));
        }

        if (request.Season.HasValue)
        {
            filteredQuery = filteredQuery.Where(h => h.Season == request.Season.Value);
        }

        var totalCount = filteredQuery.Count();
        var items = filteredQuery
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        return new PaginatedList<HymnDto>(items, totalCount, request.PageNumber, request.PageSize);
    }
}

public class PaginatedList<T>
{
    public List<T> Items { get; }
    public int PageNumber { get; }
    public int TotalPages { get; }
    public int TotalCount { get; }

    public PaginatedList(List<T> items, int count, int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        TotalCount = count;
        Items = items;
    }

    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
}
