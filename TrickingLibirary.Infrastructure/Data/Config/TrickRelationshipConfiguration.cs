using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrickingLibirary.Domain.Entities;

namespace TrickingLibirary.Infrastructure.Data.Config;

public class TrickRelationshipConfiguration : IEntityTypeConfiguration<TrickRelationship>
{
    public void Configure(EntityTypeBuilder<TrickRelationship> builder)
    {
        builder.HasKey(x => new { x.PrerequisiteId, x.ProgressionId });

        builder.HasOne(x => x.Progression)
            .WithMany(x => x.Prerequisites)
            .HasForeignKey(x => x.ProgressionId);

        builder.HasOne(x => x.Prerequisite)
           .WithMany(x => x.Progressions)
           .HasForeignKey(x => x.PrerequisiteId);
    }
}
