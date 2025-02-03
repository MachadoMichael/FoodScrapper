using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FoodScrapper.Models; // Certifique-se de incluir o namespace correto
using FoodScrapper.Services; // Inclua o namespace do FoodService

public class ScrapperService
{
    private readonly HttpClient _httpClient;
    private readonly FoodService _foodService; // Adicionando FoodService

    public ScrapperService(FoodService foodService)
    {
        _httpClient = new HttpClient();
        _foodService = foodService; // Inicializando FoodService
    }

    public async Task ScrapeFoodDataAsync(int pageNumber)
    {
        var url = $"https://www.tbca.net.br/base-dados/composicao_estatistica.php?pagina={pageNumber}&atuald=1#";
        var response = await _httpClient.GetStringAsync(url);
        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(response);

        // Seleciona a tabela que contém os dados
        var table = htmlDocument.DocumentNode.SelectSingleNode("//table"); // Ajuste o XPath conforme necessário
        var rows = table.SelectNodes(".//tr");

        foreach (var row in rows)
        {
            var columns = row.SelectNodes(".//td");
            if (columns != null && columns.Count >= 5) // Verifica se há colunas suficientes
            {
                var food = new Food
                {
                    Code = columns[0].InnerText.Trim(),
                    Name = columns[1].InnerText.Trim(),
                    ScientificName = columns[2].InnerText.Trim(),
                    Group = columns[3].InnerText.Trim(),
                    Brand = columns[4].InnerText.Trim()
                };

                // Salva o Food no banco de dados usando o FoodService
                await _foodService.CreateAsync(food);
            }
        }
    }
}
