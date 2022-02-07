using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrickingLibirary.Domain.Entities;

namespace TrickingLibirary.Infrastructure.Data.Config;

public class TrickConfiguration : IEntityTypeConfiguration<Trick>
{
    public void Configure(EntityTypeBuilder<Trick> builder)
    {
        builder.HasKey(x => new { x.Slug, x.Version });

        builder.HasMany(x => x.TrickCategories)
            .WithOne(x => x.Trick).HasForeignKey(x => new { x.TrickId, x.TrickVersion });
    }
}
