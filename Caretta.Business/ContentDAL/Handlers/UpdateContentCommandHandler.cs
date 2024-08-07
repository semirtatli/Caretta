using AutoMapper;
using Caretta.Business.Commands;
using Caretta.Business.Concrete;
using Caretta.Business.ContentDAL.Commands;
using Caretta.Core.Entity;
using Caretta.Core.Enums;
using Caretta.Core.Exceptions;
using Caretta.Data.Context;
using Hangfire;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caretta.Business.ContentDAL.Handlers
{
    public class UpdateContentCommandHandler : IRequestHandler<UpdateContentCommand, Guid>
    {
        private readonly CarettaContext _context;
        private readonly IMapper _mapper;
        private readonly UserContextService _userContextService;

        public UpdateContentCommandHandler(CarettaContext context, IMapper mapper, UserContextService userContextService)
        {
            _context = context;
            _mapper = mapper;
            _userContextService = userContextService;
        }

        public async Task<Guid> Handle(UpdateContentCommand request, CancellationToken cancellationToken)
        {




            var content = await _context.Contents
                 .Include(c => c.ContentCategories)
                     .ThenInclude(cc => cc.Category)
                     .Where(c => !c.IsDeleted)
                 .FirstOrDefaultAsync(c => c.Id == request.ContentId, cancellationToken);

            if (content == null)
            {
                throw new NotFoundException("Content to be updated not found.");
            }

            if (content.IsDeleted)
            {
                throw new DeletedException("Content to be updated is deleted.");
            }

            if (content.ApprovalStatus == ApprovalStatus.Rejected)
            {
                content.ApprovalStatus = ApprovalStatus.Draft;
                content.RejectReason = null;
            }

            _mapper.Map(request.ContentDto, content);
            content.ModifiedOn = DateTime.UtcNow;
            content.ModifiedBy = _userContextService.GetCurrentUserId();
            // Remove existing category associations
            var existingCategories = _context.ContentCategories.Where(cc => cc.ContentId == request.ContentId).ToList();
            _context.ContentCategories.RemoveRange(existingCategories);

            if (request.ContentDto.CategoryIds != null && request.ContentDto.CategoryIds.Any())
            {
                foreach (var categoryId in request.ContentDto.CategoryIds)
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

            //var categoryIds = content.ContentCategories.Select(cc => cc.CategoryId).ToList();
            //BackgroundJob.Enqueue(() => NotifyUsersOfNewContent(categoryIds, content.Id));

            await _context.SaveChangesAsync(cancellationToken);

            return content.Id;
        }
    }
}
