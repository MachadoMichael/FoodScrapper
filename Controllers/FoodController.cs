using FoodScrapper.Models;
using FoodScrapper.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodScrapper.Controllers
{
    [Route("api/foods")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        private readonly FoodService _foodService;

        public FoodController(FoodService foodService)
        {
            _foodService = foodService;
        }

        // POST: api/food
        [HttpPost]
        public async Task<ActionResult<Food>> Create(Food food)
        {
            try
            {
                if (food == null)
                {
                    return BadRequest(new { 
                        SystemMessage = "Invalid input.", 
                        UserMessage = "Please provide valid food data." 
                    });
                }

                var createdFood = await _foodService.CreateAsync(food);
                return CreatedAtAction(nameof(GetById), new { id = createdFood.Id }, createdFood);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    SystemMessage = ex.Message, 
                    UserMessage = "An error occurred while creating the food item." 
                });
            }
        }

        // GET: api/food
        [HttpGet]
        public async Task<ActionResult<List<Food>>> GetAll()
        {
            try
            {
                var foods = await _foodService.GetAllAsync();
                return Ok(foods);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    SystemMessage = ex.Message, 
                    UserMessage = "An error occurred while retrieving food items." 
                });
            }
        }

        // GET: api/food/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Food>> GetById(int id)
        {
            try
            {
                var food = await _foodService.GetByIdAsync(id);
                if (food == null)
                {
                    return NotFound(new { 
                        SystemMessage = "Food not found.", 
                        UserMessage = "The requested food item does not exist." 
                    });
                }
                return Ok(food);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    SystemMessage = ex.Message, 
                    UserMessage = "An error occurred while retrieving the food item." 
                });
            }
        }

        // PUT: api/food/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Food food)
        {
            try
            {
                if (id != food.Id)
                {
                    return BadRequest(new { 
                        SystemMessage = "ID mismatch.", 
                        UserMessage = "The provided ID does not match the food item." 
                    });
                }

                await _foodService.UpdateAsync(food);
                return NoContent();
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException e)
            {
                return NotFound(new { 
                    SystemMessage = e.Message, 
                    UserMessage = "The food item you are trying to update does not exist." 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    SystemMessage = ex.Message, 
                    UserMessage = "An error occurred while updating the food item." 
                });
            }
        }

        // DELETE: api/food/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _foodService.DeleteAsync(id);
                if (!result)
                {
                    return NotFound(new { 
                        SystemMessage = "Food not found.", 
                        UserMessage = "The food item you are trying to delete does not exist." 
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    SystemMessage = ex.Message, 
                    UserMessage = "An error occurred while deleting the food item." 
                });
            }
        }
    }
}
