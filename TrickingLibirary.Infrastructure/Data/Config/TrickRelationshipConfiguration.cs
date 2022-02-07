using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrickingLibirary.Domain.Entities;

namespace TrickingLibirary.Infrastructure.Data.Config;

public class TrickRelationshipConfiguration : IEntityTypeConfiguration<TrickRelationship>
{
    public void Configure(EntityTypeBuilder<TrickRelationship> builder)
    {
        builder.HasKey(x => new { x.PrerequisiteId, x.PrerequisiteVersion, x.ProgressionId, x.ProgressionVersion });

        builder.HasOne(x => x.Progression)
            .WithMany(x => x.Prerequisites)
            .HasForeignKey(x => new { x.ProgressionId, x.ProgressionVersion });

        builder.HasOne(x => x.Prerequisite)
           .WithMany(x => x.Progressions)
           .HasForeignKey(x => new { x.PrerequisiteId, x.PrerequisiteVersion });
    }
}
