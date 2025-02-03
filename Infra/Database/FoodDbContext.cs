using Microsoft.EntityFrameworkCore;
using FoodScrapper.Models;

namespace FoodScrapper.Infra.Database
{
    public class FoodDbContext : DbContext
    {
        public FoodDbContext(DbContextOptions<FoodDbContext> options) : base(options) { }

        public DbSet<Food> Foods { get; set; }
        public DbSet<Component> Components { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Component>()
                .HasOne(c => c.Food)
                .WithMany()
                .HasForeignKey(c => c.FoodId);
        }
    }
}
