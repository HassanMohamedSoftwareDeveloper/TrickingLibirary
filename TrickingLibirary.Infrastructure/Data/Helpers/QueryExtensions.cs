using TrickingLibirary.Domain.Entities.Abstractions;

namespace TrickingLibirary.Infrastructure.Data.Helpers;

public static class QueryExtensions
{
    public static int LatestVersion<TSource>(this IQueryable<TSource> source, int offset = 0) where TSource : VersionModel
        => source.Max(x => x.Version) + offset;
}
