using Fintacharts.Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Fintacharts.Infrastructure.Helper
{
    public static class DbExtension
    {
        public static void ApplyMigration(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();
            using DatabaseContext dbContext =
                scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            var pendingMigrations = dbContext.Database.GetPendingMigrations();
            if (pendingMigrations.Any())
            {
                dbContext.Database.Migrate();
            }
            dbContext.Database.Migrate();
        }
    }
}
