using AutoMapper;
using Caretta.Business.Commands;
using Caretta.Business.Concrete;
using Caretta.Core.Dto.CategoryDto;
using Caretta.Core.Entity;
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
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Guid>
    {
        private readonly CarettaContext _context;
        private readonly IMapper _mapper;
        private readonly UserContextService _userContextService;

        public UpdateCategoryCommandHandler(CarettaContext context, IMapper mapper, UserContextService userContextService)
        {
            _context = context;
            _mapper = mapper;
            _userContextService = userContextService;
        }

        public async Task<Guid> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {


            

               var category = await _context.Categories
                    .Include(c => c.ContentCategories)
                        .ThenInclude(cc => cc.Content)
                        .Where(c => !c.IsDeleted)
                    .FirstOrDefaultAsync(c => c.Id == request.CategoryId, cancellationToken);

            if (category == null)
                {
                    throw new NotFoundException("Category to be updated not found.");
                }

                if (category.IsDeleted)
                {
                    throw new DeletedException("Category to be updated is deleted.");
                }

                _mapper.Map(request.CategoryDto, category);
                category.ModifiedOn = DateTime.UtcNow;
                category.ModifiedBy = _userContextService.GetCurrentUserId();
                _context.Categories.Update(category);



            await _context.SaveChangesAsync(cancellationToken);

            return category.Id;
        }
    }
}