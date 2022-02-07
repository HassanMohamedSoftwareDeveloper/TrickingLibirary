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
    public void Migrate(string targetId, int targetVersion, string targetType)
    {
        var (current, next) = ResolveCurrentAndNextEntities(targetId, targetVersion, targetType);
        if (current is not null)
            current.Active = false;
        next.Active = true;
        next.Temporary = false;
    }
    #endregion

    #region Helpers :
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
