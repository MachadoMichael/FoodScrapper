using FoodScrapper.Models;
using FoodScrapper.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using FoodScrapper.Dtos;

namespace FoodScrapper.Controllers
{
    [Route("api/components")]
    [ApiController]
    public class ComponentController : ControllerBase
    {
        private readonly ComponentService _componentService;
        private readonly FoodService _foodService;

        public ComponentController(ComponentService componentService, FoodService foodService)
        {
            _componentService = componentService;
            _foodService = foodService;
        }

        // POST: api/components
        [HttpPost]
        public async Task<ActionResult<Component>> Create(NewComponentDto componentDto)
        {
            try
            {
                if (componentDto == null || componentDto.FoodId <= 0)
                {
                    return BadRequest(new { 
                        SystemMessage = "Invalid input.", 
                        UserMessage = "Please provide valid component data and a valid Food ID." 
                    });
                }

                var food = await _foodService.GetByIdAsync(componentDto.FoodId);
                if (food == null)
                {
                    return BadRequest(new { 
                        SystemMessage = "Invalid Food ID.", 
                        UserMessage = "The specified Food does not exist." 
                    });
                }

                var component = componentDto.ToModel(food);

                var createdComponent = await _componentService.CreateAsync(component);
                return CreatedAtAction(nameof(GetById), new { id = createdComponent.Id }, createdComponent);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    SystemMessage = ex.Message, 
                    UserMessage = "An error occurred while creating the component." 
                });
            }
        }

        // GET: api/components
        [HttpGet]
        public async Task<ActionResult<List<Component>>> GetAll()
        {
            try
            {
                var components = await _componentService.GetAllAsync();
                return Ok(components);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    SystemMessage = ex.Message, 
                    UserMessage = "An error occurred while retrieving components." 
                });
            }
        }

        // GET: api/components/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Component>> GetById(int id)
        {
            try
            {
                var component = await _componentService.GetByIdAsync(id);
                if (component == null)
                {
                    return NotFound(new { 
                        SystemMessage = "Component not found.", 
                        UserMessage = "The requested component does not exist." 
                    });
                }
                return Ok(component);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    SystemMessage = ex.Message, 
                    UserMessage = "An error occurred while retrieving the component." 
                });
            }
        }

        // PUT: api/components/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Component component)
        {
            try
            {
                if (id != component.Id)
                {
                    return BadRequest(new { 
                        SystemMessage = "ID mismatch.", 
                        UserMessage = "The provided ID does not match the component." 
                    });
                }

                await _componentService.UpdateAsync(component);
                return NoContent();
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException e)
            {
                return NotFound(new { 
                    SystemMessage = e.Message, 
                    UserMessage = "The component you are trying to update does not exist." 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    SystemMessage = ex.Message, 
                    UserMessage = "An error occurred while updating the component." 
                });
            }
        }

        // DELETE: api/components/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _componentService.DeleteAsync(id);
                if (!result)
                {
                    return NotFound(new { 
                        SystemMessage = "Component not found.", 
                        UserMessage = "The component you are trying to delete does not exist." 
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    SystemMessage = ex.Message, 
                    UserMessage = "An error occurred while deleting the component." 
                });
            }
        }

        // DELETE: api/components/deleteall
        [HttpDelete("deleteAll")]
        public async Task<IActionResult> DeleteAll()
        {
            try
            {
                await _componentService.DeleteAllAsync();
                return Ok(new { 
                    SystemMessage = "All components have been deleted successfully.", 
                    UserMessage = "All component data has been removed from the database." 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    SystemMessage = ex.Message, 
                    UserMessage = "An error occurred while deleting the components." 
                });
            }
        }
    }
}
