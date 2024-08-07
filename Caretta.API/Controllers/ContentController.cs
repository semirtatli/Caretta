using Microsoft.AspNetCore.Mvc;
using Caretta.Business.Abstract;
using Caretta.Core.Dto.ContentDto;
using Caretta.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Caretta.Business.Concrete;
using Microsoft.AspNetCore.Authorization;
using Caretta.Business.Queries;
using MediatR;
using Caretta.Business.ContentDAL.Queries;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Caretta.Business.Commands;
using Caretta.Core.Dto.CategoryDto;
using Caretta.Business.ContentDAL.Commands;

namespace Caretta.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContentsController : ControllerBase
    {
        private readonly IContentInterface _contentService;
        private readonly UserContextService _userContextService;
        private readonly IMediator _mediator;

        public ContentsController(IContentInterface contentService, UserContextService userContextService, IMediator mediator)
        {
            _contentService = contentService;
            _userContextService = userContextService;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("GetAllContents"), Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<ContentGetDto>>> GetAllContents()
        {
            //var contents = await _contentService.GetAllContentsAsync();
            //return Ok(contents);

            var query = new GetAllContentsQuery();
            var contents = await _mediator.Send(query);
            return Ok(contents);
        }

        [HttpGet]
        [Route("GetPopularContents"), Authorize(Roles = "user")]
        public async Task<ActionResult<IEnumerable<ContentGetDto>>> GetPopularContents()
        {
            var query = new GetPopularContentsQuery();
            var contents = await _mediator.Send(query);
            return Ok(contents);
            //var contents = await _contentService.GetPopularContentsAsync();
            //return Ok(contents);
        }

        [HttpGet]
        [Route("GetContentById"), Authorize(Roles = "admin")]
        public async Task<ActionResult<ContentGetDto>> GetContentById(Guid id)
        {
            try
            {
                var query = new GetContentByIdQuery(id);
                var content = await _mediator.Send(query);
                return Ok(content);
                //var content = await _contentService.GetContentByIdAsync(id);
                //return Ok(content);
            }
            catch (NotFoundException)
            {
                return NotFound("Content not found.");
            }
        }

        [HttpPost]
        [Route("CreateContent"), Authorize(Roles = "user")]
        public async Task<ActionResult<Guid>> CreateContent([FromBody] ContentCreateDto contentDto)
        {
            var command = new CreateContentCommand(contentDto);
            var contentId = await _mediator.Send(command);
            return Ok(contentId);
        }


        [HttpPut]
        [Route("UpdateContent"), Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateContent(Guid id, [FromBody] ContentCreateDto contentDto)
        {
            try
            {
                await _contentService.CreateOrUpdateContentAsync(id, contentDto);
                return NoContent();
            }
            catch (NotFoundException)
            {
                return NotFound($"Content with ID {id} not found.");
            }
        }

        [HttpGet]
        [Route("ApprovalWaitingContents"), Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<ContentGetDto>>> ApprovalWaitingContents()
        {
            var query = new ApprovalWaitingContentsQuery();
            var contents = await _mediator.Send(query);
            return Ok(contents);
            //var contents = await _contentService.ApprovalWaitingContentsAsync();
            //return Ok(contents);
        }

        [HttpGet]
        [Route("EditorApprovalWaitingContents"), Authorize(Roles = "editor")]
        public async Task<ActionResult<IEnumerable<ContentGetDto>>> EditorApprovalWaitingContents()
        {
            //var query = new ApprovalWaitingContentsQuery();
            //var contents = await _mediator.Send(query);
            //return Ok(contents);
            var contents = await _contentService.EditorApprovalWaitingContentsAsync();
            return Ok(contents);
        }

        [HttpGet]
        [Route("AdminApprovalWaitingContents"), Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<ContentGetDto>>> AdminApprovalWaitingContents()
        {
            //var query = new ApprovalWaitingContentsQuery();
            //var contents = await _mediator.Send(query);
            //return Ok(contents);
            var contents = await _contentService.AdminApprovalWaitingContentsAsync();
            return Ok(contents);
        }

        [HttpGet]
        [Route("ApprovalWaitingContentsForUser"), Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<ContentGetDto>>> UserApprovalWaitingContents()
        {
            var query = new ApprovalWaitingContentsForUserQuery();
            var contents = await _mediator.Send(query);
            return Ok(contents);
            //var contents = await _contentService.UserApprovalWaitingContentsAsync();
            //return Ok(contents);
        }

        [HttpPost("ApproveContent")]
        [Authorize(Roles = "editor,admin")]
        public async Task<IActionResult> ApproveContent([FromBody] Guid contentId)
        {
            
            await _contentService.ApproveContentAsync(contentId);
            return Ok();
        }

        [HttpPost("RejectContent")]
        [Authorize(Roles = "editor,admin")]
        public async Task<IActionResult> RejectContent([FromQuery] Guid contentId, [FromBody] ContentRejectDto contentRejectDto)
        {
            await _contentService.RejectContentAsync(contentId, contentRejectDto);
            return Ok();
        }


        [HttpPost("LikeContent")]
        [Authorize(Roles = "editor,admin")]
        public async Task<IActionResult> LikeContent(Guid contentId)
        {
            try
            {
                var content = await _contentService.LikeContentAsync(contentId);
                return Ok(content);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (DeletedException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }/*
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while liking the content.", detail = ex.Message });
            }*/
        }


        [HttpDelete("UnlikeContent")]
        [Authorize(Roles = "editor,admin")]
        public async Task<IActionResult> UnlikeContent(Guid contentId)
        {
            try
            {
                var content = await _contentService.UnlikeContentAsync(contentId);
                return Ok(content);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (DeletedException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }/*
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while liking the content.", detail = ex.Message });
            }*/
        }

        [HttpDelete]
        [Route("DeleteContent"), Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteContent(Guid id)
        {
            try
            {
                await _contentService.DeleteContentAsync(id);
                return NoContent();
            }
            catch (NotFoundException)
            {
                return NotFound($"Content with ID {id} not found.");
            }
        }
    }
}
