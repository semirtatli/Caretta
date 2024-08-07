using Microsoft.AspNetCore.Mvc;
using Caretta.Core.Dto;
using Caretta.Business.Abstract;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Caretta.API.Controllers
{
    [ApiController]
    [Route("api/contentCategories")]
    public class ContentCategoriesController : ControllerBase
    {
        private readonly IContentCategoriesInterface _contentCategoriesService;

        public ContentCategoriesController(IContentCategoriesInterface contentCategoriesService)
        {
            _contentCategoriesService = contentCategoriesService;
        }

        [HttpGet("GetAllContentCategories")]
        public async Task<ActionResult<IEnumerable<ContentCategoryDto>>> GetAllContentCategories()
        {
            var contentCategories = await _contentCategoriesService.GetAllContentCategoriesAsync();
            return Ok(contentCategories);
        }

        [HttpPost("AddContentCategory")]
        public async Task<ActionResult> AddContentCategory([FromBody] ContentCategoryDto contentCategoryDto)
        {
            await _contentCategoriesService.AddContentCategoryAsync(contentCategoryDto);
            return NoContent();
        }

        [HttpDelete("RemoveContentCategory")]
        public async Task<ActionResult> RemoveContentCategory(Guid contentId, Guid categoryId)
        {
            await _contentCategoriesService.RemoveContentCategoryAsync(contentId, categoryId);
            return NoContent();
        }
    }
}

