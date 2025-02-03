namespace FoodScrapper.Models
{
    public class Food
    {
        public int Id { get; set; } // Primary key
        public string Code { get; set; }
        public string Name { get; set; }
        public string ScientificName { get; set; }
        public string Group { get; set; }
        public string Brand { get; set; } // Added Brand property
    }
}
