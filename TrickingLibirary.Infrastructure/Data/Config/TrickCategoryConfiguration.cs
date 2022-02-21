using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrickingLibirary.Domain.Entities;

namespace TrickingLibirary.Infrastructure.Data.Config;

public class TrickCategoryConfiguration : IEntityTypeConfiguration<TrickCategory>
{
    public void Configure(EntityTypeBuilder<TrickCategory> builder)
    {
        builder.HasKey(x => new { x.CategoryId, x.TrickId });
    }
}
