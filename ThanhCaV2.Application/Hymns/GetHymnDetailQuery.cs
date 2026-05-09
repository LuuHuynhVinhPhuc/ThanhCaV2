using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ThanhCaV2.Application.Commons;

namespace ThanhCaV2.Application.Hymns;

public record GetHymnDetailQuery(Guid Id) : IRequest<HymnDetailDto?>;

public class GetHymnDetailQueryHandler : IRequestHandler<GetHymnDetailQuery, HymnDetailDto?>
{
    private readonly IApplicationDbContext _context;

    public GetHymnDetailQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<HymnDetailDto?> Handle(GetHymnDetailQuery request, CancellationToken cancellationToken)
    {
        return await _context.Hymns
            .AsNoTracking()
            .Include(h => h.Sections)
            .Where(h => h.Id == request.Id)
            .ProjectToType<HymnDetailDto>()
            .FirstOrDefaultAsync(cancellationToken);
    }
}
