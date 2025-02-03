using FoodScrapper.Models;

namespace FoodScrapper.Dtos
{
    public class NewComponentDto
    {
        public string Name { get; set; }
        public string Units { get; set; }
        public float ValuePer100g { get; set; }
        public string StandardDeviation { get; set; }
        public float? MinValue { get; set; }
        public float? MaxValue { get; set; }
        public int? NumberOfDataUsed { get; set; }
        public string References { get; set; }
        public DataType DataType { get; set; }
        public int FoodId { get; set; }

        public Component ToModel(Food food)
        {
            return new Component
            {
                Name = this.Name,
                Units = this.Units,
                ValuePer100g = this.ValuePer100g,
                StandardDeviation = this.StandardDeviation,
                MinValue = this.MinValue,
                MaxValue = this.MaxValue,
                NumberOfDataUsed = this.NumberOfDataUsed,
                References = this.References,
                DataType = this.DataType,
                FoodId = this.FoodId,
                Food = food
            };
        }
    }
}
