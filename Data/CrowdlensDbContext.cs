using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Crowdlens_backend.Models;

namespace CrowdLens.Data;


public class CrowdLensDbContext : IdentityDbContext<User>
{
    public CrowdLensDbContext(DbContextOptions<CrowdLensDbContext> options) 
        : base(options) { }

    public DbSet<Area> Areas { get; set; } // why is there a need to do this? what does this do? I read that DbSet<Area> recognizes the model Area into a schema or table?
    public DbSet<Establishment> Establishments { get; set; }
    public DbSet<UserAreaSubscription> UserAreaSubscriptions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure UserAreaSubscription relationships
        modelBuilder.Entity<UserAreaSubscription>()
            .HasOne(uas => uas.User)
            .WithMany()
            .HasForeignKey(uas => uas.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserAreaSubscription>()
            .HasOne(uas => uas.Area)
            .WithMany()
            .HasForeignKey(uas => uas.AreaId)
            .OnDelete(DeleteBehavior.Cascade);

        // Create a unique constraint on (UserId, AreaId) to prevent duplicate subscriptions
        modelBuilder.Entity<UserAreaSubscription>()
            .HasIndex(uas => new { uas.UserId, uas.AreaId })
            .IsUnique();
    }
}