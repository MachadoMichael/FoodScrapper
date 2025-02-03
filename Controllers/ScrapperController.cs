using FoodScrapper.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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

        // POST: api/scrapper/scrape
        [HttpPost("foods")]
        public async Task<IActionResult> Scrape(int pageNumber = 1)
        {
            try
            {
                await _scrapperService.ScrapeFoodDataAsync(pageNumber);
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
            try
            {
                await _scrapperService.ScrapeComponentsForFoodsAsync();
                return Ok(new { 
                    SystemMessage = "Components scraping completed successfully.", 
                    UserMessage = "Component data has been scraped and saved to the database." 
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
