using System.Reflection;
using Fintacharts.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Fintacharts.Infrastructure.Persistence
{
    public class DatabaseContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Asset> Assets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
