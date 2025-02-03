namespace FoodScrapper.Models
{
    using System.Text.Json.Serialization;

    public class Component
    {
        public int Id { get; set; } // Primary key
        public string Name { get; set; } // Nome
        public string Units { get; set; } // Unidades
        public float ValuePer100g { get; set; } // Valor por 100g
        public string StandardDeviation { get; set; } // Desvio padrão
        public float? MinValue { get; set; } // Valor mínimo (nullable)
        public float? MaxValue { get; set; } // Valor máximo (nullable)
        public int? NumberOfDataUsed { get; set; } // Número de dados utilizados (nullable)
        public string References { get; set; } // Referências
        public DataType DataType { get; set; } // Tipo de dados

        // Relação com Food
        public int FoodId { get; set; } // Chave estrangeira
        public Food Food { get; set; } // Propriedade de navegação
    }

    public enum DataType
    {
        Calculated,
        Analytical,
        Assumed
    }
}

