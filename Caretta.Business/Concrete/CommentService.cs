using AutoMapper;
using Caretta.Business.Abstract;
using Caretta.Core.Entity;
using Caretta.Core.Exceptions;
using Caretta.Core.Dto.CommentDto;
using Caretta.Data.Context;
using Microsoft.EntityFrameworkCore;
using Caretta.Core.Dto.ContentDto;
using System.Collections.Generic;
using System.Xml.Linq;
using Caretta.Core.Enums;

namespace Caretta.Business.Concrete
{
    public class CommentService : ICommentInterface
    {
        private readonly CarettaContext _context;
        private readonly IMapper _mapper;
        private readonly UserContextService _userContextService;

        public CommentService(CarettaContext context, IMapper mapper, UserContextService userContextService)
        {
            _context = context;
            _mapper = mapper;
            _userContextService = userContextService;
        }

        public async Task<IEnumerable<CommentGetDto>> GetAllCommentsAsync()
        {
            var comments = await _context.Comments
                .Include(c => c.Content)
                .Where(c => !c.IsDeleted)
                .OrderByDescending(c => c.CreatedOn)
                .ToListAsync();

            return _mapper.Map<IEnumerable<CommentGetDto>>(comments);
        }

        public async Task<CommentGetDto> GetCommentByIdAsync(Guid commentId)
        {
            var comment = await _context.Comments
                .Include(c => c.Content)
                .OrderByDescending(c => c.CreatedOn)
                .FirstOrDefaultAsync(c => c.Id == commentId);

            if (comment == null)
                throw new NotFoundException("Comment not found.");
            if (comment.IsDeleted)
            {
                throw new DeletedException("Comment is deleted.");
            }

            return _mapper.Map<CommentGetDto>(comment);
        }

        public async Task<IEnumerable<ContentWithCommentsDto>> GetAllCommentsByUserAsync(Guid userId)
        {
            var comments = await _context.Comments
                .Include(c => c.Content)
                .Where(c => !c.IsDeleted && c.CreatedBy == userId)
                .OrderByDescending(c => c.CreatedOn)
                .ToListAsync();

            var groupedComments = comments
                .GroupBy(c => c.Content)
                .Select(g => new ContentWithCommentsDto
                {
                    Content = _mapper.Map<ContentGetDto>(g.Key),
                    Comments = _mapper.Map<IEnumerable<CommentGetDto>>(g)
                })
                .ToList();

            return groupedComments;
        }

        public async Task<CommentCreateDto> CreateOrUpdateCommentAsync(Guid? commentId, CommentCreateDto commentDto)
        {
            Comment comment;
            Content content = await _context.Contents.FirstOrDefaultAsync(c => c.Id == commentDto.ContentId);

            if (commentId.HasValue)
            {
                comment = await _context.Comments
                    .Include(c => c.Content)
                    .FirstOrDefaultAsync(c => c.Id == commentId.Value && !c.IsDeleted);

                if (comment == null)
                    throw new NotFoundException("Comment to be updated not found.");
                if (comment.IsDeleted)
                    throw new DeletedException("Comment is deleted.");

                _mapper.Map(commentDto, comment);
                comment.ModifiedOn = DateTime.UtcNow;
                comment.ModifiedBy = _userContextService.GetCurrentUserId();
                _context.Comments.Update(comment);
            }
            else
            {
                comment = _mapper.Map<Comment>(commentDto);
                comment.CreatedOn = DateTime.UtcNow;
                comment.CreatedBy = _userContextService.GetCurrentUserId();
                if (content.ContentStatus == ContentStatus.Draft) {
                throw new ContentNotPublishedException("You can not comment on a draft content");
                }
                if (!content.IsApproved)
                {
                    throw new Exception("You can not comment on an unapproved content");
                }
                _context.Comments.Add(comment);

                content.CommentCount++;
                _context.Contents.Update(content);

            }

            await _context.SaveChangesAsync();
            return _mapper.Map<CommentCreateDto>(comment);
        }

        /*
        public async Task<CommentCreateDto> CreateCommentAsync(CommentCreateDto commentDto)
        {
            var comment = _mapper.Map<Comment>(commentDto);
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return _mapper.Map<CommentCreateDto>(comment);
        }

        public async Task UpdateCommentAsync(Guid commentId, CommentCreateDto commentDto)
        {
            var comment = await _context.Comments
                .Include(c => c.Content)
                .FirstOrDefaultAsync(c => c.Id == commentId && !c.IsDeleted);

            if (comment == null)
                throw new NotFoundException("Comment to be updated not found.");
            if (comment.IsDeleted)
            {
                throw new DeletedException("Comment is deleted.");
            }

            _mapper.Map(commentDto, comment);
            comment.ModifiedOn = DateTime.UtcNow;

            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
        }
        */

        public async Task<CommentGetDto> LikeCommentAsync(Guid commentId)
        {
            var userId = _userContextService.GetCurrentUserId();

            var comment = await _context.Comments
                .Include(c => c.UserLikeComments)
                .ThenInclude(cc => cc.User)
                //.Include(u => u.Comments.Where(comment => comment.IsApproved))
                .FirstOrDefaultAsync(c => c.Id == commentId);

            if (comment == null)
                throw new NotFoundException("Comment not found.");
            if (comment.IsDeleted)
            {
                throw new DeletedException("Comment is deleted.");
            }

            var existingLike = await _context.UserLikeComments
        .FirstOrDefaultAsync(ulc => ulc.UserId == userId && ulc.CommentId == commentId);

            if (existingLike != null)
            {
                throw new InvalidOperationException("You have already liked this content.");
            }

            if (comment.Id != null)
            {

                var userLikeContent = new UserLikeContents
                {
                    UserId = userId,
                    ContentId = commentId,
                    LikeDate = DateTime.UtcNow
                };
                _context.UserLikeContents.Add(userLikeContent);

                comment.LikesCount++;

            }
            await _context.SaveChangesAsync();

            return _mapper.Map<CommentGetDto>(comment);
        }
        public async Task<IEnumerable<CommentApproveDto>> ApproveCommentsAsync(Guid? commentId)//, CommentApproveUpdateDto? commentDto)
        {
            if (commentId.HasValue) {

                Comment comment = await _context.Comments
                    .Include(c => c.Content)
                    .FirstOrDefaultAsync(c => c.Id == commentId.Value && !c.IsDeleted && c.IsApproved == false);

                if (comment == null)
                    throw new NotFoundException("Comment to be updated not found.");
                if (comment.IsDeleted)
                    throw new DeletedException("Comment is deleted.");

                //_mapper.Map(commentDto, comment);
                comment.IsApproved = true;
                comment.ModifiedOn = DateTime.UtcNow;
                comment.ModifiedBy = _userContextService.GetCurrentUserId();
                _context.Comments.Update(comment);

                await _context.SaveChangesAsync();
                //return _mapper.Map<IEnumerable<CommentApproveDto>>(comment);
            }
            
            var comments = await _context.Comments
                .Include(c => c.Content)
                .Where(c => !c.IsDeleted && c.IsApproved == false)
                .OrderByDescending(c => c.CreatedOn)
                .ToListAsync();

                
            
            return _mapper.Map<IEnumerable<CommentApproveDto>>(comments);

        }
        /*
        public async Task<CommentApproveUpdateDto> CommentApproveUpdateAsync(Guid? commentId, CommentCreateDto commentDto)
        {

        }
        */
        public async Task DeleteCommentAsync(Guid commentId)
        {
            var comment = await _context.Comments
                .Include(c => c.Content)
                .FirstOrDefaultAsync(c => c.Id == commentId);

            if (comment == null)
                throw new NotFoundException("Comment to be deleted not found.");
            if (comment.IsDeleted)
            {
                throw new DeletedException("Comment is deleted.");
            }
            comment.IsDeleted = true;

            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
        }
    }
}
