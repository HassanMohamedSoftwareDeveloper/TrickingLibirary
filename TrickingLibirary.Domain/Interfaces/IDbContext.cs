using Microsoft.EntityFrameworkCore;
using TrickingLibirary.Domain.Entities;

namespace TrickingLibirary.Domain.Interfaces;

public interface IDbContext
{
    #region PROPS :
    public DbSet<Trick> Tricks { get; set; }
    public DbSet<Submission> Submissions { get; set; }
    public DbSet<Difficulty> Difficulties { get; set; }
    public DbSet<TrickRelationship> TrickRelationships { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<TrickCategory> TrickCategories { get; set; }
    #endregion
    #region Methods :
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    #endregion
}
