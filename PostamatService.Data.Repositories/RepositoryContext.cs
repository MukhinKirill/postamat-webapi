using Microsoft.EntityFrameworkCore;
using PostamatService.Data.Models;
using PostamatService.Data.Repositories.Configurations;

namespace PostamatService.Data.Repositories
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new PostamatConfiguration());
        }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Postamat> Postamats { get; set; }
    }
}
