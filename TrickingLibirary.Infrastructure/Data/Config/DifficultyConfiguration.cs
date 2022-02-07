using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrickingLibirary.Domain.Entities;

namespace TrickingLibirary.Infrastructure.Data.Config;
public class DifficultyConfiguration : IEntityTypeConfiguration<Difficulty>
{
    public void Configure(EntityTypeBuilder<Difficulty> builder)
    {
        builder.HasKey(x => new { x.Slug, x.Version });
    }
}
