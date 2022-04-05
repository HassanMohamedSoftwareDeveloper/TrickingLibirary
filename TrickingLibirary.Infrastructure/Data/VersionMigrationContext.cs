using TrickingLibirary.Domain.Entities.Abstractions;
using TrickingLibirary.Domain.Entities.Modertion;
using TrickingLibirary.Domain.Interfaces;

namespace TrickingLibirary.Infrastructure.Data;

public class VersionMigrationContext
{
    #region Fields :
    private readonly IDbContext dbContext;
    #endregion

    #region CTORS :
    public VersionMigrationContext(IDbContext dbContext) => this.dbContext = dbContext;
    #endregion

    #region Methods :
    public void Migrate(ModerationItem modItem)
    {
        var source = GetSource(modItem.Type);

        var current = source.FirstOrDefault(x => x.Id.Equals(modItem.Current));
        var target = source.FirstOrDefault(x => x.Id.Equals(modItem.Target));

        if (target is null) throw new InvalidOperationException("Target not found");

        if (current is not null)
        {
            if (target.Version - current.Version <= 0)
            {
                throw new InvalidVersionException($"Current version is {current.Version}, Target version is {target.Version}, for {modItem.Type}.");
            }
            current.Active = false;

            var outdatedModerationItems = dbContext.ModerationItems
                .Where(x => x.Deleted.Equals(false) && x.Type == modItem.Type && !x.Id.Equals(modItem.Id)).ToList();

            foreach (var outdatedModerationItem in outdatedModerationItems)
            {
                outdatedModerationItem.Current = target.Id;
            }
        }
        target.Active = true;

        MigrateRelationships(modItem.Current, modItem.Target, modItem.Type);
    }
    #endregion

    #region Helpers :
    private IQueryable<VersionModel> GetSource(string type)
    {
        if (type.Equals(MpderationTypes.Trick))
        {
            return dbContext.Tricks;
        }
        throw new ArgumentException(null, nameof(type));
    }
    private void MigrateRelationships(int current, int target, string type)
    {
        if (type.Equals(MpderationTypes.Trick))
        {
            if (current > 0)
            {
                dbContext.TrickRelationships
                    .Where(x => x.PrerequisiteId == current || x.ProgressionId == current)
                    .ToList()
                    .ForEach(x => x.Active = false);
            }
            dbContext.TrickRelationships
                   .Where(x => x.PrerequisiteId == target || x.ProgressionId == target)
                   .ToList()
                   .ForEach(x => x.Active = false);
        }
        else
            throw new ArgumentException(null, nameof(type));
    }
    private (VersionModel Current, VersionModel Next) ResolveCurrentAndNextEntities(string targetId, int targetVersion, string targetType)
    {
        if (targetType.Equals(MpderationTypes.Trick))
        {
            var current = dbContext.Tricks.FirstOrDefault(x => x.Slug.Equals(targetId) && x.Active);
            var next = dbContext.Tricks.FirstOrDefault(x => x.Slug.Equals(targetId) && x.Version == targetVersion);
            return (current, next);
        }
        throw new ArgumentException(null, nameof(targetType));
    }
    #endregion
}
public class InvalidVersionException : Exception
{
    public InvalidVersionException(string message) : base(message) { }
}
