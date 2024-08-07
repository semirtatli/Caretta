using AutoMapper;
using Caretta.Business.ContentDAL.Queries;
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

namespace Caretta.Business.ContentDAL.Handlers
{

    public class GetContentByIdQueryHandler : IRequestHandler<GetContentByIdQuery, ContentGetByIdDto>
    {
        private readonly CarettaContext _context;
        private readonly IMapper _mapper;

        public GetContentByIdQueryHandler(CarettaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ContentGetByIdDto> Handle(GetContentByIdQuery request, CancellationToken cancellationToken)
        {
            var content = await _context.Contents
                .Include(c => c.ContentCategories)
                .ThenInclude(cc => cc.Category)
                .Include(u => u.Comments.Where(comment => comment.IsApproved))
                .FirstOrDefaultAsync(c => c.Id == request.contentId);

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
    }
}
