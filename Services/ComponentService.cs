using FoodScrapper.Infra.Database;
using FoodScrapper.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodScrapper.Services
{
    public class ComponentService
    {
        private readonly FoodDbContext _context;

        public ComponentService(FoodDbContext context)
        {
            _context = context;
        }

        // Create
        public async Task<Component> CreateAsync(Component component)
        {
            _context.Components.Add(component);
            await _context.SaveChangesAsync();
            return component;
        }

        // Read
        public async Task<List<Component>> GetAllAsync()
        {
            return await _context.Components.Include(c => c.Food).ToListAsync(); // Inclui a relação com Food
        }

        public async Task<Component> GetByIdAsync(int id)
        {
            return await _context.Components.Include(c => c.Food).FirstOrDefaultAsync(c => c.Id == id); // Inclui a relação com Food
        }

        // Update
        public async Task<Component> UpdateAsync(Component component)
        {
            _context.Entry(component).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return component;
        }

        // Delete
        public async Task<bool> DeleteAsync(int id)
        {
            var component = await _context.Components.FindAsync(id);
            if (component == null)
            {
                return false;
            }

            _context.Components.Remove(component);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
