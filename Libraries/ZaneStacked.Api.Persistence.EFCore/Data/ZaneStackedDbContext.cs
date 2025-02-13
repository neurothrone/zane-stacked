using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ZaneStacked.Api.Persistence.EFCore.Models;
using ZaneStacked.Api.Persistence.Shared.Models;

namespace ZaneStacked.Api.Persistence.EFCore.Data;

public class ZaneStackedDbContext : IdentityDbContext<AppUser>
{
    public ZaneStackedDbContext(DbContextOptions<ZaneStackedDbContext> options) : base(options)
    {
    }

    public DbSet<Skill> Skills => Set<Skill>();
    public DbSet<Project> Projects => Set<Project>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Skill>()
            .HasMany(s => s.Projects)
            .WithMany(p => p.Skills);
    }
}