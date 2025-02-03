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

        // POST: api/scrapper/scrape?page={pageNumber}
        [HttpPost("run")]
        public async Task<IActionResult> Run(int pageNumber = 1)
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
    }
}
