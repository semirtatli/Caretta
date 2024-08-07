using Microsoft.AspNetCore.Mvc;
using Caretta.Business.Abstract;
using Caretta.Core.Dto.CommentDto;
using Caretta.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Caretta.Business.Concrete;
using Microsoft.AspNetCore.Authorization;

namespace Caretta.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentInterface _commentService;

        public CommentsController(ICommentInterface commentService)
        {
            _commentService = commentService;
        }

        [HttpGet]
        [Route("GetAllComments"), Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<CommentGetDto>>> GetAllComments()
        {
            var comments = await _commentService.GetAllCommentsAsync();
            return Ok(comments);
        }

        [HttpGet]
        [Route("GetCommentById"), Authorize(Roles = "admin")]
        public async Task<ActionResult<CommentGetDto>> GetCommentById(Guid id)
        {
            try
            {
                var comment = await _commentService.GetCommentByIdAsync(id);
                return Ok(comment);
            }
            catch (NotFoundException)
            {
                return NotFound("Comment not found.");
            }
        }

        [HttpGet]
        [Route("GetAllCommentsByUser"), Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<CommentGetDto>>> GetAllCommentsByUser(Guid id)
        {
            var comments = await _commentService.GetAllCommentsByUserAsync(id);
            return Ok(comments);
        }

        [HttpPost]
        [Route("CreateComment"), Authorize(Roles = "User")]
        public async Task<ActionResult<CommentCreateDto>> CreateComment([FromBody] CommentCreateDto commentDto)
        {
            var createdComment = await _commentService.CreateOrUpdateCommentAsync(null, commentDto);
            return Ok();
        }

        [HttpGet]
        [Route("GetUnapprovedComments"), Authorize(Roles = "admin")]
        
        public async Task<ActionResult<IEnumerable<CommentApproveDto>>> GetUnapprovedCommentsAsnyc()
        {
            var comments = await _commentService.ApproveCommentsAsync(null);//, null);
            return Ok(comments);
        }

        [HttpPut]
        [Route("ApproveComments"), Authorize(Roles = "admin")]
        public async Task<IActionResult> ApproveUpdateComment(Guid id)//, [FromBody] CommentApproveUpdateDto commentDto)
        {
            try
            {
                await _commentService.ApproveCommentsAsync(id);//, commentDto);
                return NoContent();
            }
            catch (NotFoundException)
            {
                return NotFound($"Comment with ID {id} not found.");
            }
        }

        [HttpPost("LikeComment")]
        [Authorize(Roles = "editor,admin")]
        public async Task<IActionResult> LikeComment(Guid commentId)
        {
            try
            {
                var comment = await _commentService.LikeCommentAsync(commentId);
                return Ok(comment);
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

        [HttpPut]
        [Route("UpdateComment"), Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateComment(Guid id, [FromBody] CommentCreateDto commentDto)
        {
            try
            {
                await _commentService.CreateOrUpdateCommentAsync(id, commentDto);
                return NoContent();
            }
            catch (NotFoundException)
            {
                return NotFound($"Comment with ID {id} not found.");
            }
        }

        [HttpDelete]
        [Route("DeleteComment"), Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteComment(Guid id)
        {
            try
            {
                await _commentService.DeleteCommentAsync(id);
                return NoContent();
            }
            catch (NotFoundException)
            {
                return NotFound($"Comment with ID {id} not found.");
            }
        }
    }
}
