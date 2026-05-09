using Microsoft.EntityFrameworkCore;
using ThanhCaV2.Domain.Models;
using ThanhCaV2.Share.Enums;

namespace ThanhCaV2.Infrastructure.Persistances;

public static class DbInitializer
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        if (!await context.Hymns.AnyAsync())
        {
            var hymn1 = new Hymn
            {
                Id = Guid.NewGuid(),
                Title = "Cầu Cho Cha Mẹ 1",
                Author = "Phanxicô",
                Season = HymnSeason.ThuongNien,
                Notes = "Bài hát tâm tình",
                Sections = new List<LyricSection>
                {
                    new LyricSection { Order = 1, Type = LyricType.PhienKhuc, Label = "1", Content = "Xin Chúa chúc lành cho cha mẹ con..." },
                    new LyricSection { Order = 2, Type = LyricType.DiepKhuc, Label = "", Content = "Công ơn cha mẹ như biển rộng trời cao..." }
                }
            };

            context.Hymns.Add(hymn1);
            await context.SaveChangesAsync();
        }
    }
}
