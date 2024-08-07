using Microsoft.AspNetCore.Mvc;
using Caretta.Business.Abstract;
using System;
using Caretta.Core.Exceptions;
using Caretta.Core.Dto.CategoryDto;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using Caretta.Business.Commands;
using Caretta.Business.Queries;
using Caretta.Core.Entity;

namespace Caretta.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        public readonly ICategoryInterface _categoryService;
        private readonly IMediator _mediator;

        public CategoriesController(ICategoryInterface categoryService, IMediator mediator)
        {
            _categoryService = categoryService;
            _mediator = mediator;
        }
        /*
        [HttpGet] 
        [Route("api/categories/GetAllCategories"), Authorize(Roles = "basic")]
        public async Task<ActionResult<IEnumerable<CategoryGetDto>>> GetAllCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }
        */
        [HttpGet("GetAllCategories"), Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<CategoryGetDto>>> GetAllCategories()
        {
            var query = new GetAllCategoriesQuery();
            var categories = await _mediator.Send(query);
            return Ok(categories);
        }
        [HttpGet]
        [Route("GetCategoryById"), Authorize(Roles = "admin")]
        public async Task<ActionResult<CategoryGetDto>> GetCategoryById(Guid id)
        {
            try
            {
                //var category = await _categoryService.GetCategoryByIdAsync(id);
                //return Ok(category);
                var query = new GetCategoryByIdQuery(id);
                var categories = await _mediator.Send(query);
                return Ok(categories);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (DeletedException ex)
            {
                return StatusCode(410, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the category.", detail = ex.Message });
            }
            /*
            catch (NotFoundException)
            {
                return NotFound($"Category not found.");
            }
            */
        }

        [HttpPost("CreateCategory"), Authorize(Roles = "admin")]
        public async Task<ActionResult<Guid>> CreateCategory([FromBody] CategoryCreateDto categoryDto)
        {
            var command = new CreateCategoryCommand(categoryDto);
            var categoryId = await _mediator.Send(command);
            return Ok(categoryId);
        }

        /*
        [HttpPost("CreateCategory"), Authorize(Roles = "all")]
        public async Task<ActionResult<CategoryCreateDto>> CreateCategory([FromBody] CategoryCreateDto categoryDto)
        {
            var createdCategory = await _categoryService.CreateOrUpdateCategoryAsync(null, categoryDto);
            return Ok();
         //   return CreatedAtAction(nameof(GetCategoryById), new { id = Id }, createdCategory);
        }
        */


        [HttpPut("UpdateCategory"), Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateCategory(Guid categoryId, [FromBody] CategoryCreateDto categoryDto)
        {
            try
            {
                var command = new UpdateCategoryCommand(categoryId, categoryDto);
                var updatedCategoryId = await _mediator.Send(command);
                return Ok(updatedCategoryId);
            }
            catch (NotFoundException)
            {
                return NotFound($"Category with ID {categoryId} not found.");
            }
            catch (DeletedException ex)
            {
                return StatusCode(410, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the category.", detail = ex.Message });
            }
        }
        
        [HttpDelete]
        [Route("DeleteCategory"), Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            try
            {
                var query = new DeleteCategoryCommand(id);
                var categories = await _mediator.Send(query);
                return Ok(categories);
                //await _categoryService.DeleteCategoryAsync(id);
                //return NoContent();
            }
            catch (NotFoundException)
            {
                return NotFound($"Category with ID {id} not found.");
            }
        }

        
        [HttpPost("AddFavoriteCategory"), Authorize(Roles = "User")]
        public async Task<IActionResult> AddFavouriteCategory(Guid categoryId)
        {
            try
            {
                var query = new AddFavouriteCategoryCommand(categoryId);
                var categories = await _mediator.Send(query);
                return Ok(categories);
                //await _categoryService.AddFavouriteCategoryAsync(categoryId);
                //return Ok();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        

    }
}
