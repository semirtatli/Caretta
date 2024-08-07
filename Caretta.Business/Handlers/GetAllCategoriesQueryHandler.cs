using AutoMapper;
using Caretta.Business.Queries;
using Caretta.Core.Dto.CategoryDto;
using Caretta.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caretta.Business.Handlers
{
    public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, IEnumerable<CategoryGetDto>>
    {
        private readonly CarettaContext _context;
        private readonly IMapper _mapper;

        public GetAllCategoriesQueryHandler(CarettaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryGetDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await _context.Categories
                .Include(c => c.ContentCategories)
                .ThenInclude(cc => cc.Content)
                .Where(c => !c.IsDeleted)
                .ToListAsync();

            return _mapper.Map<IEnumerable<CategoryGetDto>>(categories);
        }
    }
}
