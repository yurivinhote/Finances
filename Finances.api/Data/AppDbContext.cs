using Finances.api.Data.Mappings;
using Finances.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Transactions;

namespace Finances.api.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
          modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Transactions> Transactions { get; set; } = null!; 
    }
}
