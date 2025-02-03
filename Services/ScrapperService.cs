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
        _httpClient = new HttpClient
        {
            Timeout = TimeSpan.FromMinutes(5) // Aumenta o timeout para 5 minutos
        };
        _foodService = foodService; // Inicializando FoodService
        _componentService = componentService; // Inicializando ComponentService
    }

    public async Task ScrapeFoodDataAsync()
    {
        int pageNumber = 1;
        bool hasMoreData = true;
        var foodsToSave = new List<Food>(); // Lista para armazenar os alimentos

        while (hasMoreData)
        {
            var url = $"https://www.tbca.net.br/base-dados/composicao_estatistica.php?pagina={pageNumber}&atuald=1#";
            var response = await _httpClient.GetStringAsync(url);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(response);

            // Seleciona a tabela que contém os dados
            var table = htmlDocument.DocumentNode.SelectSingleNode("//table"); // Ajuste o XPath conforme necessário
            var rows = table.SelectNodes(".//tr");

            if (rows == null || rows.Count <= 1) // Se não houver linhas ou apenas o cabeçalho
            {
                hasMoreData = false; // Não há mais dados para processar
                break; // Sai do loop
            }

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

                    foodsToSave.Add(food); // Adiciona o Food à lista
                }
            }

            pageNumber++; // Incrementa o número da página para a próxima iteração
        }

        // Salva todos os Foods no banco de dados de uma só vez
        await _foodService.CreateRangeAsync(foodsToSave); // Método para salvar em massa

        // Após o loop, você pode retornar uma mensagem de sucesso
        Console.WriteLine("Scraping completed successfully."); // Log para depuração
    }

    public async Task ScrapeComponentsForFoodsAsync()
    {
        // Busca todas as Foods do banco de dados
        var foods = await _foodService.GetAllAsync();
        var componentsToSave = new List<Component>(); // Lista para armazenar os componentes

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

                        componentsToSave.Add(component); // Adiciona o Component à lista
                    }
                    catch (FormatException ex)
                    {
                        // Log de erro para verificar qual valor causou o problema
                        Console.WriteLine($"Error parsing component data for Food ID {food.Id}: {ex.Message}");
                    }
                }
            }
        }

        // Salva todos os Components no banco de dados de uma só vez
        await _componentService.CreateRangeAsync(componentsToSave); // Método para salvar em massa
    }

}

