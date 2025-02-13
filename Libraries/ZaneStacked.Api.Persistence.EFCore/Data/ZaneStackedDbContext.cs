using Microsoft.EntityFrameworkCore;
using ZaneStacked.Api.Persistence.EFCore.Models;

namespace ZaneStacked.Api.Persistence.EFCore.Data;

public class ZaneStackedDbContext : DbContext
{
    public ZaneStackedDbContext(DbContextOptions<ZaneStackedDbContext> options) : base(options)
    {
    }

    public DbSet<Skill> Skills => Set<Skill>();
    public DbSet<Project> Projects => Set<Project>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Skill>()
            .HasMany(s => s.Projects)
            .WithMany(p => p.Skills);

        base.OnModelCreating(modelBuilder);
    }
}