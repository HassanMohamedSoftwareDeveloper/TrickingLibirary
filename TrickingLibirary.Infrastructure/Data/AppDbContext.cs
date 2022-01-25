using Microsoft.EntityFrameworkCore;
using TrickingLibirary.Domain.Entities;
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
    #endregion
}
