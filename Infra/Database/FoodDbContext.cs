using Microsoft.EntityFrameworkCore;
using FoodScrapper.Models;

namespace FoodScrapper.Infra.Database
{
    public class FoodDbContext : DbContext
    {
        public FoodDbContext(DbContextOptions<FoodDbContext> options) : base(options) { }

        public DbSet<Food> Foods { get; set; }
    }
}
