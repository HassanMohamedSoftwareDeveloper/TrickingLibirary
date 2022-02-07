using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrickingLibirary.Domain.Entities;

namespace TrickingLibirary.Infrastructure.Data.Config;
public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(x => new { x.Slug, x.Version });

        builder.HasMany(x => x.Tricks)
            .WithOne(x => x.Category).HasForeignKey(x => new { x.CategoryId, x.CategoryVersion});
    }
}
