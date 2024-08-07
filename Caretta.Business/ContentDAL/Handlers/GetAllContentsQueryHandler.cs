using AutoMapper;
    using Caretta.Data.Context;
    using Caretta.Core.Dto.ContentDto;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Caretta.Business.ContentDAL.Queries;
using Caretta.Core.Enums;

namespace Caretta.Business.ContentDAL.Handlers
{
    public class GetAllContentsQueryHandler : IRequestHandler<GetAllContentsQuery, IEnumerable<ContentGetDto>>
        {
            private readonly CarettaContext _context;
            private readonly IMapper _mapper;

            public GetAllContentsQueryHandler(CarettaContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<IEnumerable<ContentGetDto>> Handle(GetAllContentsQuery request, CancellationToken cancellationToken)
            {
            var contents = await _context.Contents
                .Include(c => c.ContentCategories)
                .ThenInclude(cc => cc.Category)
                .Where(c => !c.IsDeleted && c.ContentStatus == ContentStatus.Published && c.IsApproved)
                .ToListAsync();

            var contentDtos = _mapper.Map<IEnumerable<ContentGetDto>>(contents);

            foreach (var contentDto in contentDtos)
            {
                contentDto.ApprovalStatusDescription = contentDto.ApprovalStatus.GetDescription();
            }

            return contentDtos;

            //return _mapper.Map<IEnumerable<ContentGetDto>>(contents);
            }
        }
    }
