using AutoMapper;
using Caretta.Business.ContentDAL.Queries;
using Caretta.Core.Dto.ContentDto;
using Caretta.Core.Enums;
using Caretta.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caretta.Business.ContentDAL.Handlers
{
    public class GetPopularContentsQueryHandler : IRequestHandler<GetPopularContentsQuery, IEnumerable<ContentGetDto>>
    {
        private readonly CarettaContext _context;
        private readonly IMapper _mapper;

        public GetPopularContentsQueryHandler(CarettaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ContentGetDto>> Handle(GetPopularContentsQuery request, CancellationToken cancellationToken)
        {
            var contents = await _context.Contents
                 .Include(c => c.ContentCategories)
                 .ThenInclude(cc => cc.Category)
                 .Where(c => !c.IsDeleted && c.IsApproved && c.CommentCount > 1)
                 .Where(c => c.ContentStatus == ContentStatus.Published)
                 .ToListAsync();
            return _mapper.Map<IEnumerable<ContentGetDto>>(contents);
        }
    }
}