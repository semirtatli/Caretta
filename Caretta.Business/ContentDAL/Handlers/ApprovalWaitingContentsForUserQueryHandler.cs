using AutoMapper;
using Caretta.Data.Context;
using Caretta.Core.Dto.ContentDto;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Caretta.Business.ContentDAL.Queries;
using Caretta.Core.Enums;
using Caretta.Business.Concrete;

namespace Caretta.Business.ContentDAL.Handlers
{
    public class ApprovalWaitingContentsForUserQueryHandler : IRequestHandler<ApprovalWaitingContentsForUserQuery, IEnumerable<ContentGetApprovalDto>>
    {
        private readonly CarettaContext _context;
        private readonly IMapper _mapper;
        private readonly UserContextService _userContextService;

        public ApprovalWaitingContentsForUserQueryHandler(CarettaContext context, IMapper mapper, UserContextService userContextService)
        {
            _context = context;
            _mapper = mapper;
            _userContextService = userContextService;
        }

        public async Task<IEnumerable<ContentGetApprovalDto>> Handle(ApprovalWaitingContentsForUserQuery request, CancellationToken cancellationToken)
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
    }
}
