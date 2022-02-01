using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TrickingLibirary.Domain.Entities;
using TrickingLibirary.Domain.Entities.Modertion;
using TrickingLibirary.Domain.Interfaces;

namespace TrickingLibirary.Infrastructure.Data;

public class AppDbContext : DbContext,IDbContext
{
    #region CTORS :
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    #endregion
    #region PROPS :
    public DbSet<Trick> Tricks { get; set; }
    public DbSet<Submission> Submissions { get; set; }
    public DbSet<Difficulty> Difficulties { get; set; }
    public DbSet<TrickRelationship> TrickRelationships { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<TrickCategory> TrickCategories { get; set; }
    public DbSet<Video> Videos { get; set; }
    public DbSet<ModerationItem> ModerationItems { get; set; }
    #endregion
    #region Methods :
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
    #endregion
}
