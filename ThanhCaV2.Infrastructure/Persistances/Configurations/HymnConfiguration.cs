using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThanhCaV2.Domain.Models;

namespace ThanhCaV2.Infrastructure.Persistances.Configurations;

public class HymnConfiguration : IEntityTypeConfiguration<Hymn>
{
    public void Configure(EntityTypeBuilder<Hymn> builder)
    {
        builder.ToTable("hymns");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id");

        builder.Property(x => x.Title).HasColumnName("title").HasMaxLength(255).IsRequired();
        builder.Property(x => x.Author).HasColumnName("author").HasMaxLength(255);
        builder.Property(x => x.Season).HasColumnName("season").HasConversion<string>();
        builder.Property(x => x.Notes).HasColumnName("notes");

        builder.Property(x => x.CreatedAt).HasColumnName("created_at");
        builder.Property(x => x.CreatedBy).HasColumnName("created_by");
        builder.Property(x => x.LastModifiedAt).HasColumnName("last_modified_at");
        builder.Property(x => x.LastModifiedBy).HasColumnName("last_modified_by");
        builder.Property(x => x.IsDeleted).HasColumnName("is_deleted").HasDefaultValue(false);
        builder.Property(x => x.DeletedAt).HasColumnName("deleted_at");

        builder.HasMany(x => x.Sections)
            .WithOne(x => x.Hymn)
            .HasForeignKey(x => x.HymnId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
