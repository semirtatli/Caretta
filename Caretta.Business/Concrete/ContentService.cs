using AutoMapper;
using Caretta.Business.Abstract;
using Caretta.Core.Entity;
using Caretta.Core.Exceptions;
using Caretta.Core.Dto.ContentDto;
using Caretta.Data.Context;
using Microsoft.EntityFrameworkCore;
using Hangfire;
using Caretta.Core.Dto.EmailDto;
using Caretta.Core.Enums;
using Caretta.Core.Dto.CategoryDto;

namespace Caretta.Business.Concrete
{
    public class ContentService : IContentInterface
    {
        private readonly CarettaContext _context;
        private readonly IMapper _mapper;
        private readonly IEmailInterface _emailService;
        private readonly ICommentInterface _commentService;
        private readonly UserContextService _userContextService;

        public ContentService(CarettaContext context, IMapper mapper, IEmailInterface emailService, UserContextService userContextService, ICommentInterface commentService)
        {
            _context = context;
            _mapper = mapper;
            _emailService = emailService;
            _userContextService = userContextService;
            _commentService = commentService;
        }

        public async Task<IEnumerable<ContentGetDto>> GetAllContentsAsync()
        {

            var contents = await _context.Contents
                .Include(c => c.ContentCategories)
                .ThenInclude(cc => cc.Category)
                .Where(c => !c.IsDeleted && c.ContentStatus == ContentStatus.Published && c.IsApproved)
                .ToListAsync();

            

            return _mapper.Map<IEnumerable<ContentGetDto>>(contents);
        }

        public async Task<ContentGetByIdDto> GetContentByIdAsync(Guid contentId)
        {
            var content = await _context.Contents
                .Include(c => c.ContentCategories)
                .ThenInclude(cc => cc.Category)
                .Include(u => u.Comments.Where(comment => comment.IsApproved))
                .FirstOrDefaultAsync(c => c.Id == contentId);

            if (content == null)
                throw new NotFoundException("Content not found.");
            if (content.IsDeleted)
            {
                throw new DeletedException("Content is deleted.");
            }

            var contentDto = _mapper.Map<ContentGetByIdDto>(content);
            contentDto.Categories = content.ContentCategories
                .Select(cc => _mapper.Map<CategoryGetDto>(cc.Category))
                .ToList();

            return contentDto;
        }

        public async Task<IEnumerable<ContentGetDto>> GetPopularContentsAsync()
        {

            var contents = await _context.Contents
                .Include(c => c.ContentCategories)
                .ThenInclude(cc => cc.Category)
                .Where(c => !c.IsDeleted)
                .Where(c  => c.CommentCount>1)
                .ToListAsync();
            return _mapper.Map<IEnumerable<ContentGetDto>>(contents);
        }

        public async Task<ContentCreateDto> CreateOrUpdateContentAsync(Guid? contentId, ContentCreateDto contentDto)
        {
            Content content;

            if (contentId.HasValue)
            {
                content = await _context.Contents
                    .Include(c => c.ContentCategories)
                    .FirstOrDefaultAsync(c => c.Id == contentId.Value);

                if (content == null)
                    throw new NotFoundException("Content to be updated not found.");
                if (content.IsDeleted)
                    throw new DeletedException("Content is deleted.");

                if (content.ApprovalStatus == ApprovalStatus.Rejected) 
                {
                    content.ApprovalStatus = ApprovalStatus.Pending;
                    content.RejectReason = null;
                }

                _mapper.Map(contentDto, content);
                content.ModifiedOn = DateTime.UtcNow;
                content.ModifiedBy = _userContextService.GetCurrentUserId();
                // Remove existing category associations
                var existingCategories = _context.ContentCategories.Where(cc => cc.ContentId == contentId.Value).ToList();
                _context.ContentCategories.RemoveRange(existingCategories);
            }
            else
            {
                content = _mapper.Map<Content>(contentDto);
                content.CreatedOn = DateTime.UtcNow;
                content.CreatedBy = _userContextService.GetCurrentUserId();
                _context.Contents.Add(content);
            }

            await _context.SaveChangesAsync();

            
            if (contentDto.CategoryIds != null && contentDto.CategoryIds.Any())
            {
                foreach (var categoryId in contentDto.CategoryIds)
                {
                    var contentCategory = new ContentCategories
                    {
                        ContentId = content.Id,
                        CategoryId = categoryId
                    };
                    _context.ContentCategories.Add(contentCategory);
                }
            }

            await _context.SaveChangesAsync();
            
            var categoryIds = content.ContentCategories.Select(cc => cc.CategoryId).ToList();
            BackgroundJob.Enqueue(() => NotifyUsersOfNewContent(categoryIds, content.Id));

            return _mapper.Map<ContentCreateDto>(content);
        }


        public async Task<ContentGetByIdDto> LikeContentAsync(Guid contentId) 
        {
            var userId = _userContextService.GetCurrentUserId();

            var content = await _context.Contents
                .Include(c => c.UserLikeContents)
                .ThenInclude(cc => cc.User)
                .Include(u => u.Comments.Where(comment => comment.IsApproved))
                .FirstOrDefaultAsync(c => c.Id == contentId);

            if (content == null)
                throw new NotFoundException("Content not found.");
            if (content.IsDeleted)
            {
                throw new DeletedException("Content is deleted.");
            }
            
            var existingLike = await _context.UserLikeContents
        .FirstOrDefaultAsync(ulc => ulc.UserId == userId && ulc.ContentId == contentId);

            if (existingLike != null)
            {
                throw new InvalidOperationException("You have already liked this content.");
            }

            if (content.Id != null)
            {  
                
                    var userLikeContent = new UserLikeContents
                    {
                        UserId = userId,
                        ContentId = contentId,
                        LikeDate = DateTime.UtcNow
                    };
                    _context.UserLikeContents.Add(userLikeContent);

                content.LikesCount++;
                
            }
            await _context.SaveChangesAsync();

            return _mapper.Map<ContentGetByIdDto>(content);
        }

        public async Task<ContentGetByIdDto> UnlikeContentAsync(Guid contentId)
        {
            var userId = _userContextService.GetCurrentUserId();

            var content = await _context.Contents
                .Include(c => c.UserLikeContents)
                .ThenInclude(cc => cc.User)
                .Include(u => u.Comments.Where(comment => comment.IsApproved))
                .FirstOrDefaultAsync(c => c.Id == contentId);

            if (content == null)
                throw new NotFoundException("Content not found.");
            if (content.IsDeleted)
            {
                throw new DeletedException("Content is deleted.");
            }

            var existingLike = await _context.UserLikeContents
        .FirstOrDefaultAsync(ulc => ulc.UserId == userId && ulc.ContentId == contentId);

            if (existingLike == null)
            {
                throw new InvalidOperationException("You didnt liked this content.");
            }

            if (content.Id != null)
            {

                _context.UserLikeContents.Remove(existingLike);

                content.LikesCount--;

            }
            await _context.SaveChangesAsync();

            return _mapper.Map<ContentGetByIdDto>(content);
        }
        public async Task<IEnumerable<ContentGetApprovalDto>> ApprovalWaitingContentsAsync()
        {

            var contents = await _context.Contents
                .Include(c => c.ContentCategories)
                .ThenInclude(cc => cc.Category)
                .Where(c => !c.IsDeleted && c.ContentStatus == ContentStatus.Published && !c.IsApproved)
                .ToListAsync();



            return _mapper.Map<IEnumerable<ContentGetApprovalDto>>(contents);
        }

        public async Task<IEnumerable<ContentGetApprovalDto>> EditorApprovalWaitingContentsAsync()
        {

            var contents = await _context.Contents
                .Include(c => c.ContentCategories)
                .ThenInclude(cc => cc.Category)
                .Where(c => !c.IsDeleted && c.ContentStatus == ContentStatus.Published && !c.IsApproved
                && c.ApprovalStatus == ApprovalStatus.Pending)
                .ToListAsync();



            return _mapper.Map<IEnumerable<ContentGetApprovalDto>>(contents);
        }

        public async Task<IEnumerable<ContentGetApprovalDto>> AdminApprovalWaitingContentsAsync()
        {

            var contents = await _context.Contents
                .Include(c => c.ContentCategories)
                .ThenInclude(cc => cc.Category)
                .Where(c => !c.IsDeleted && c.ContentStatus == ContentStatus.Published && !c.IsApproved
                && c.ApprovalStatus == ApprovalStatus.EditorApproved)
                .ToListAsync();



            return _mapper.Map<IEnumerable<ContentGetApprovalDto>>(contents);
        }

        public async Task<IEnumerable<ContentGetApprovalDto>> UserApprovalWaitingContentsAsync()
        {
            Guid userId = _userContextService.GetCurrentUserId();

            var contents = await _context.Contents
                .Include(c => c.ContentCategories)
                .ThenInclude(cc => cc.Category)
                .Where(c => !c.IsDeleted && c.ContentStatus == ContentStatus.Published && !c.IsApproved
                && userId == c.CreatedBy)
                .ToListAsync();



            return _mapper.Map<IEnumerable<ContentGetApprovalDto>>(contents);
        }

        public async Task ApproveContentAsync(Guid contentId)
        {
            

            Guid userId = _userContextService.GetCurrentUserId();

            var user = await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new NotFoundException("User not found.");

            var roles = user.UserRoles.Select(ur => ur.Role.RoleType).ToList();

            if (!roles.Any(r => r.Equals("Editor", StringComparison.OrdinalIgnoreCase) || r.Equals("Admin", StringComparison.OrdinalIgnoreCase)))
            {
                throw new UnauthorizedAccessException("User does not have the required role to approve or reject content.");
            }

            string role = roles.Contains("admin") ? "Admin" : "Editor";

            var content = await _context.Contents.FirstOrDefaultAsync(c => c.Id == contentId && !c.IsDeleted);

            if (content == null)
                throw new NotFoundException("Content not found.");
            if (content.IsDeleted)
                throw new DeletedException("Content is deleted.");

            if (role.Equals("Editor", StringComparison.OrdinalIgnoreCase))
            {
                if (content.ApprovalStatus == ApprovalStatus.Pending)
                {
                    content.ApprovalStatus = ApprovalStatus.EditorApproved;
                }
                else if (content.ApprovalStatus == ApprovalStatus.Rejected)
                {
                    throw new Exception("Content has rejected and must be updated for approval.");
                }
                else
                {
                    throw new Exception("Content has already been approved by the editor or higher.");
                }
            }
            else if (role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                if (content.ApprovalStatus == ApprovalStatus.EditorApproved)
                {
                    content.ApprovalStatus = ApprovalStatus.AdminApproved;
                    content.IsApproved = true;
                }
                else if (content.ApprovalStatus == ApprovalStatus.Rejected)
                {
                    throw new Exception("Content has rejected and must be updated for approval.");
                }
                else
                {
                    throw new Exception("Content must be approved by the editor first.");
                }
            }

            content.ModifiedOn = DateTime.UtcNow;
            content.ModifiedBy = userId;
            _context.Contents.Update(content);

            await _context.SaveChangesAsync();
        }

        public async Task RejectContentAsync(Guid contentId, ContentRejectDto contentRejectDto)
        {


            Guid userId = _userContextService.GetCurrentUserId();

            var user = await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new NotFoundException("User not found.");

            var roles = user.UserRoles.Select(ur => ur.Role.RoleType).ToList();

            if (!roles.Any(r => r.Equals("Editor", StringComparison.OrdinalIgnoreCase) || r.Equals("Admin", StringComparison.OrdinalIgnoreCase)))
            {
                throw new UnauthorizedAccessException("User does not have the required role to approve or reject content.");
            }

            string role = roles.Contains("admin") ? "Admin" : "Editor";

            var content = await _context.Contents.FirstOrDefaultAsync(c => c.Id == contentId && !c.IsDeleted);

            if (content == null)
                throw new NotFoundException("Content not found.");
            if (content.IsDeleted)
                throw new DeletedException("Content is deleted.");

            if (role.Equals("Editor", StringComparison.OrdinalIgnoreCase))
            {
                if (content.ApprovalStatus == ApprovalStatus.Pending)
                {
                    content.ApprovalStatus = ApprovalStatus.Rejected;
                    content.RejectReason = contentRejectDto.RejectReason;
                }
                else
                {
                    throw new Exception("Content has already been approved by the editor or higher.");
                }
            }
            else if (role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                if (content.ApprovalStatus == ApprovalStatus.EditorApproved)
                {
                    content.ApprovalStatus = ApprovalStatus.Rejected;
                    content.RejectReason = contentRejectDto.RejectReason;
                }
                else
                {
                    throw new Exception("Content must be approved by the editor first.");
                }
            }

            content.ModifiedOn = DateTime.UtcNow;
            content.ModifiedBy = userId;
            _context.Contents.Update(content);

            await _context.SaveChangesAsync();
        }

        public async Task NotifyUsersOfNewContent(List<Guid> categoryIds, Guid contentId)
        {
            var users = await _context.Users
                .Include(u => u.UserFavouriteCategories)
                .Where(u => u.UserFavouriteCategories.Any(ufc => categoryIds.Contains(ufc.CategoryId)))
                .ToListAsync();

            foreach (var user in users)
            {
                var emailDto = new EmailDto
                {
                    To = user.Email,
                    Subject = "New Content Added",
                    Body = $"A new content has been added in one of your favorite categories: {contentId}"
                };
                BackgroundJob.Enqueue(() => _emailService.SendEmailAsync(emailDto));
            }
        }

        public async Task UpdateContentStatusAsync()
        {
            // Get the current UTC time rounded to the nearest minute
            var currentTime = DateTime.UtcNow;

            // Fetch all contents with Draft status where PublishDate matches the current time up to minutes
            var contentsToPublish = await _context.Contents
                .Where(c => c.ContentStatus == ContentStatus.Draft
                            && c.PublishDate <= currentTime
                            && c.IsApproved == true
                            && !c.IsDeleted)
                .ToListAsync();

            foreach (var content in contentsToPublish)
            {
                content.ContentStatus = ContentStatus.Published;
            }

            if (contentsToPublish.Count > 0)
            {
                await _context.SaveChangesAsync();
            }
        }

        public async Task SendContentToApprovalAsync(Guid Id)
        {
            // Get the current UTC time rounded to the nearest minute
            var content = await _context.Contents
                .Include(c => c.ContentCategories)
                .ThenInclude(cc => cc.Category)
                .Include(u => u.Comments.Where(comment => comment.IsApproved))
                .FirstOrDefaultAsync(c => c.Id == Id);

            if (content == null)
                throw new NotFoundException("Content not found.");
            if (content.IsDeleted)
            {
                throw new DeletedException("Content is deleted.");
            }

            if (content.ApprovalStatus == ApprovalStatus.Draft) {
                content.ApprovalStatus = ApprovalStatus.Pending;
            }

            var contentDto = _mapper.Map<ContentGetByIdDto>(content);
            contentDto.Categories = content.ContentCategories
                .Select(cc => _mapper.Map<CategoryGetDto>(cc.Category))
                .ToList();
        }


        /*
        public async Task<ContentCreateDto> CreateOrUpdateContentAsync(Guid? contentId, ContentCreateDto contentDto)
        {
            Content content;

            if (contentId.HasValue)
            {
                content = await _context.Contents
                    .Include(c => c.ContentCategories)
                    .FirstOrDefaultAsync(c => c.Id == contentId.Value);

                if (content == null)
                    throw new NotFoundException("Content to be updated not found.");
                if (content.IsDeleted)
                    throw new DeletedException("Content is deleted.");

                content.Title = contentDto.Title;
                content.Body = contentDto.Body;
                content.ModifiedOn = DateTime.UtcNow;

                
                var existingCategories = _context.ContentCategories.Where(cc => cc.ContentId == contentId.Value).ToList();
                _context.ContentCategories.RemoveRange(existingCategories);
            }
            else
            {
                content = new Content
                {
                    Title = contentDto.Title,
                    Body = contentDto.Body
                };

                _context.Contents.Add(content);
            }

            await _context.SaveChangesAsync();

            // Add new category associations
            if (contentDto.CategoryIds != null && contentDto.CategoryIds.Any())
            {
                foreach (var categoryId in contentDto.CategoryIds)
                {
                    var contentCategory = new ContentCategories
                    {
                        ContentId = content.Id,
                        CategoryId = categoryId
                    };
                    _context.ContentCategories.Add(contentCategory);
                }
            }

            await _context.SaveChangesAsync();

            return contentDto;
        }

        */
        public async Task DeleteContentAsync(Guid contentId)
        {
            var content = await _context.Contents
                .Include(c => c.ContentCategories)
                .ThenInclude(cc => cc.Category)
                .FirstOrDefaultAsync(c => c.Id == contentId);

            if (content == null)
                throw new NotFoundException("Content to be deleted not found.");
            if (content.IsDeleted)
            {
                throw new DeletedException("Content is deleted.");
            }

            foreach (var comment in content.Comments)
            {
                await _commentService.DeleteCommentAsync(comment.Id);
            }

            content.IsDeleted = true;

            _context.Contents.Update(content);
            await _context.SaveChangesAsync();
        }
    }
}
