using Microsoft.EntityFrameworkCore;
using ZaneStacked.Api.Persistence.EFCore.Data;

namespace ZaneStacked.Api.Extensions;

public static class MigrationExtensions
{
    public static async Task ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();
        await using var context = scope.ServiceProvider.GetRequiredService<ZaneStackedDbContext>();
        await context.Database.MigrateAsync();
    }
}