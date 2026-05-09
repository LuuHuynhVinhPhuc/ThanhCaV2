using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThanhCaV2.Domain.Models;

namespace ThanhCaV2.Infrastructure.Persistances.Configurations;

public class LyricSectionConfiguration : IEntityTypeConfiguration<LyricSection>
{
    public void Configure(EntityTypeBuilder<LyricSection> builder)
    {
        builder.ToTable("lyric_sections");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id");

        builder.Property(x => x.HymnId).HasColumnName("hymn_id");
        builder.Property(x => x.Order).HasColumnName("order");
        builder.Property(x => x.Type).HasColumnName("type").HasConversion<string>();
        builder.Property(x => x.Label).HasColumnName("label").HasMaxLength(50);
        builder.Property(x => x.Content).HasColumnName("content").IsRequired();
    }
}
