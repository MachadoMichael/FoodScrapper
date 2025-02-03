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

        public DataType GetDataTypeFromText(string text)
        {
            var dataTypeMap = new Dictionary<string, DataType>
        {
            { "Analítico", DataType.Analytical },
            { "Calculado", DataType.Calculated },
            { "Assumido", DataType.Assumed }
        };

            if (dataTypeMap.TryGetValue(text, out DataType dataType))
            {
                return dataType;
            }

            // Try to parse the input text directly as enum if not found in dictionary
            if (Enum.TryParse<DataType>(text, true, out DataType result))
            {
                return result;
            }

            return DataType.Assumed; // Default value if text is not found and can't be parsed
        }

        // Método para apagar todos os Components
        public async Task DeleteAllAsync()
        {
            _context.Components.RemoveRange(_context.Components);
            await _context.SaveChangesAsync();

            // Reset identity/sequence to 1
            await _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE \"Components\" RESTART IDENTITY CASCADE");
        }

        // Método para salvar uma lista de Components
        public async Task CreateRangeAsync(List<Component> components)
        {
            await _context.Components.AddRangeAsync(components); // Adiciona todos os Components à lista
            await _context.SaveChangesAsync(); // Salva as alterações no banco de dados
        }
    }
}
