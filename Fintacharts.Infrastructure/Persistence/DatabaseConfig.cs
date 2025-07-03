using Fintacharts.Infrastructure.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Fintacharts.Infrastructure.Persistence
{
    public class DatabaseConfig(IOptions<ConnectionOptions> options) : DbContext
    {
        private readonly IOptions<ConnectionOptions> _options = options;

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseNpgsql(_options.Value.ApiDatabase);
        }

    }
}