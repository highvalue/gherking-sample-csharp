using Gherkin.Contract.OrderCheckingAPI;
using Microsoft.EntityFrameworkCore;

namespace Gherkin.Core.OrderCheckingAPI.Provider
{
    public class OrderCheckingContext : DbContext
    {
        public OrderCheckingContext(DbContextOptions<OrderCheckingContext> options) : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StockItem>().HasKey(x => new { x.Id });          
        }

        public DbSet<StockItem> StockItems { get; set; }      
    }
}
