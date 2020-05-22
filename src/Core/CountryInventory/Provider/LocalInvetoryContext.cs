using Microsoft.EntityFrameworkCore;

namespace Gherkin.Core.CentralHub.Provider
{
    public class LocalInvetoryContext : DbContext
    {
        public LocalInvetoryContext(DbContextOptions<LocalInvetoryContext> options) : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LocalStockItem>().HasKey(x => new { x.Id });
        }

        public DbSet<LocalStockItem> LocalStockItems { get; set; }
    }
}