using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace tomware.Microsts.Web
{
  public class STSContext : IdentityDbContext<ApplicationUser>
  {
    public DbSet<ClaimType> ClaimTypes { get; set; }

    public STSContext(DbContextOptions<STSContext> options) : base(options)
    {
      this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);

      builder.Entity<ClaimType>()
          .HasIndex(c => c.Name)
          .IsUnique();
    }
  }
}