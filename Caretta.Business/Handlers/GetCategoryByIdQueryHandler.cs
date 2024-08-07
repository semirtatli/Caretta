using AutoMapper;
using Caretta.Business.Queries;
using Caretta.Core.Dto.CategoryDto;
using Caretta.Core.Dto.ContentDto;
using Caretta.Core.Entity;
using Caretta.Core.Enums;
using Caretta.Core.Exceptions;
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

    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, CategoryGetByIdDto>
    {
        private readonly CarettaContext _context;
        private readonly IMapper _mapper;

        public GetCategoryByIdQueryHandler(CarettaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CategoryGetByIdDto> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var category = await _context.Categories
                .Where(c => c.Id == request.categoryId && !c.IsDeleted)
                .Include(c => c.ContentCategories)
                    .ThenInclude(cc => cc.Content)
                .FirstOrDefaultAsync(cancellationToken);

            if (category == null)
            {
                throw new NotFoundException("Category not found.");
            }

            var categoryDto = _mapper.Map<CategoryGetByIdDto>(category);
            categoryDto.Contents = category.ContentCategories
                .Where(cc => cc.Content.ContentStatus == ContentStatus.Published && cc.Content.IsApproved)
                .Select(cc => _mapper.Map<ContentGetCategoryDto>(cc.Content))
                .ToList();

            return categoryDto;
        }
    }
}
