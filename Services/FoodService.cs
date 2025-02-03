using YourNamespace.Data; // Adjust the namespace according to your project structure
using YourNamespace.Models; // Adjust the namespace according to your project structure
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace YourNamespace.Services
{
    public class FoodService
    {
        private readonly FoodDbContext _context;

        public FoodService(FoodDbContext context)
        {
            _context = context;
        }

        // Create
        public async Task<Food> CreateAsync(Food food)
        {
            _context.Foods.Add(food);
            await _context.SaveChangesAsync();
            return food;
        }

        // Read
        public async Task<List<Food>> GetAllAsync()
        {
            return await _context.Foods.ToListAsync();
        }

        public async Task<Food> GetByIdAsync(int id)
        {
            return await _context.Foods.FindAsync(id);
        }

        // Update
        public async Task<Food> UpdateAsync(Food food)
        {
            _context.Entry(food).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return food;
        }

        // Delete
        public async Task<bool> DeleteAsync(int id)
        {
            var food = await _context.Foods.FindAsync(id);
            if (food == null)
            {
                return false;
            }

            _context.Foods.Remove(food);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
