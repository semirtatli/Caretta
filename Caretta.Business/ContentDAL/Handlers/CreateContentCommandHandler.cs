using AutoMapper;
using Caretta.Business.Commands;
using Caretta.Business.Concrete;
using Caretta.Business.ContentDAL.Commands;
using Caretta.Core.Entity;
using Caretta.Data.Context;
using Hangfire;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Caretta.Business.Handlers
{
    public class CreateContentCommandHandler : IRequestHandler<CreateContentCommand, Guid>
    {
        private readonly CarettaContext _context;
        private readonly IMapper _mapper;
        private readonly UserContextService _userContextService;

        public CreateContentCommandHandler(CarettaContext context, IMapper mapper, UserContextService userContextService)
        {
            _context = context;
            _mapper = mapper;
            _userContextService = userContextService;
        }

        public async Task<Guid> Handle(CreateContentCommand request, CancellationToken cancellationToken)
        {
            var content = _mapper.Map<Content>(request.ContentDto);
            content.CreatedOn = DateTime.UtcNow;
            content.CreatedBy = _userContextService.GetCurrentUserId();

            _context.Contents.Add(content);
            await _context.SaveChangesAsync(cancellationToken);

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

            await _context.SaveChangesAsync(cancellationToken);

            //var categoryIds = content.ContentCategories.Select(cc => cc.CategoryId).ToList();
            //BackgroundJob.Enqueue(() => NotifyUsersOfNewContent(categoryIds, content.Id));

            return content.Id;
        }
    }
}
