using AutoMapper;
using Caretta.Data.Context;
using Caretta.Core.Dto.ContentDto;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Caretta.Business.ContentDAL.Queries;
using Caretta.Core.Enums;

namespace Caretta.Business.ContentDAL.Handlers
{
    public class ApprovalWaitingContentsQueryHandler : IRequestHandler<ApprovalWaitingContentsQuery, IEnumerable<ContentGetApprovalDto>>
    {
        private readonly CarettaContext _context;
        private readonly IMapper _mapper;

        public ApprovalWaitingContentsQueryHandler(CarettaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ContentGetApprovalDto>> Handle(ApprovalWaitingContentsQuery request, CancellationToken cancellationToken)
        {
            var contents = await _context.Contents
                .Include(c => c.ContentCategories)
                .ThenInclude(cc => cc.Category)
                .Where(c => !c.IsDeleted && c.ContentStatus == ContentStatus.Published && !c.IsApproved)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ContentGetApprovalDto>>(contents);
        }
    }
}
