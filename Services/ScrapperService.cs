using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FoodScrapper.Models; // Certifique-se de incluir o namespace correto
using FoodScrapper.Services; // Inclua o namespace do FoodService e ComponentService
using Microsoft.EntityFrameworkCore; // Para usar o DbContext

public class ScrapperService
{
    private readonly HttpClient _httpClient;
    private readonly FoodService _foodService; // Adicionando FoodService
    private readonly ComponentService _componentService; // Adicionando ComponentService

    public ScrapperService(FoodService foodService, ComponentService componentService)
    {
        _httpClient = new HttpClient();
        _foodService = foodService; // Inicializando FoodService
        _componentService = componentService; // Inicializando ComponentService
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

    public async Task ScrapeComponentsForFoodsAsync()
    {
        // Busca todas as Foods do banco de dados
        var foods = await _foodService.GetAllAsync();

        foreach (var food in foods)
        {
            var url = $"https://www.tbca.net.br/base-dados/int_composicao_estatistica.php?cod_produto={food.Code}";
            var response = await _httpClient.GetStringAsync(url);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(response);

            // Seleciona a tabela que contém os dados dos componentes
            var componentTable = htmlDocument.DocumentNode.SelectSingleNode("//table"); // Ajuste o XPath conforme necessário
            var componentRows = componentTable.SelectNodes(".//tr");

            foreach (var componentRow in componentRows)
            {
                var componentColumns = componentRow.SelectNodes(".//td");
                if (componentColumns != null && componentColumns.Count >= 8) // Verifica se há colunas suficientes
                {
                    try
                    {
                        var component = new Component
                        {
                            Name = componentColumns[0].InnerText.Trim(),
                            Units = componentColumns[1].InnerText.Trim(),
                            ValuePer100g = float.Parse(componentColumns[2].InnerText.Trim().Replace(",", ".")), // Ajuste para o formato correto
                            StandardDeviation = componentColumns[3].InnerText.Trim(),
                            MinValue = string.IsNullOrEmpty(componentColumns[4].InnerText.Trim()) ? (float?)null : float.Parse(componentColumns[4].InnerText.Trim().Replace(",", ".")),
                            MaxValue = string.IsNullOrEmpty(componentColumns[5].InnerText.Trim()) ? (float?)null : float.Parse(componentColumns[5].InnerText.Trim().Replace(",", ".")),
                            NumberOfDataUsed = string.IsNullOrEmpty(componentColumns[6].InnerText.Trim()) ? (int?)null : int.Parse(componentColumns[6].InnerText.Trim()),
                            References = componentColumns[7].InnerText.Trim(),
                            // Usando o método GetDataTypeFromText para converter o texto em DataType
                            DataType = _componentService.GetDataTypeFromText(componentColumns[8].InnerText.Trim()), // Converte o tipo de dados
                            FoodId = food.Id // Vincula o Component ao Food
                        };

                        // Salva o Component no banco de dados usando o ComponentService
                        await _componentService.CreateAsync(component);
                    }
                    catch (FormatException ex)
                    {
                        // Log de erro para verificar qual valor causou o problema
                        Console.WriteLine($"Error parsing component data for Food ID {food.Id}: {ex.Message}");
                    }
                }
            }
        }
    }

}

