using FoodScrapper.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using System.Timers;

namespace FoodScrapper.Controllers
{
    [Route("api/scrapper")]
    [ApiController]
    public class ScrapperController : ControllerBase
    {
        private readonly ScrapperService _scrapperService;

        public ScrapperController(ScrapperService scrapperService)
        {
            _scrapperService = scrapperService;
        }

        [HttpPost("foods")]
        public async Task<IActionResult> Scrape()
        {
            try
            {
                await _scrapperService.ScrapeFoodDataAsync();
                return Ok(new { 
                    SystemMessage = "Scraping completed successfully.", 
                    UserMessage = "Food data has been scraped and saved to the database." 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    SystemMessage = ex.Message, 
                    UserMessage = "An error occurred while scraping the data." 
                });
            }
        }

        // POST: api/scrapper/components
        [HttpPost("components")]
        public async Task<IActionResult> ScrapeComponents()
        {
            // Define um tempo limite de 5 minutos para a requisição
            var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMinutes(5));
            
            try
            {
                await _scrapperService.ScrapeComponentsForFoodsAsync();
                return Ok(new { 
                    SystemMessage = "Components scraping completed successfully.", 
                    UserMessage = "Component data has been scraped and saved to the database." 
                });
            }
            catch (OperationCanceledException)
            {
                return StatusCode(408, new { 
                    SystemMessage = "Request timed out.", 
                    UserMessage = "The operation took too long and was canceled." 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    SystemMessage = ex.Message, 
                    UserMessage = "An error occurred while scraping the component data." 
                });
            }
        }
    }
}
