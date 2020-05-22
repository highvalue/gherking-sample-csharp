using Microsoft.EntityFrameworkCore;

namespace Gherkin.Core.CentralHub.Provider
{
    public class CentralInvetoryContext : DbContext
    {
        public CentralInvetoryContext(DbContextOptions<CentralInvetoryContext> options) : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CentralStockItem>().HasKey(x => new { x.Id });
        }

        public DbSet<CentralStockItem> CentralStockItems { get; set; }
    }
}