using AutoMapper;
using Caretta.Business.Commands;
using Caretta.Business.Concrete;
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
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Guid>
    {
        private readonly CarettaContext _context;
        private readonly IMapper _mapper;
        private readonly UserContextService _userContextService;

        public DeleteCategoryCommandHandler(CarettaContext context, IMapper mapper, UserContextService userContextService)
        {
            _context = context;
            _mapper = mapper;
            _userContextService = userContextService;
        }

        public async Task<Guid> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {




            var category = await _context.Categories
                .Include(c => c.ContentCategories)
                    .ThenInclude(cc => cc.Content)
                .FirstOrDefaultAsync(c => c.Id == request.CategoryId);

            if (category == null)
            {
                throw new NotFoundException("Category to be deleted not found.");
            }
            if (category.IsDeleted)
            {
                throw new DeletedException("Category to be deleted is already deleted.");
            }

            category.IsDeleted = true;
            _context.Categories.Update(category);

            // Check related contents
            foreach (var contentCategory in category.ContentCategories)
            {
                var content = contentCategory.Content;
                if (content.ContentCategories.Count == 1)
                {
                    //await _contentService.DeleteContentAsync(content.Id);
                    // //content.IsDeleted = true;
                   // //contextCategory.Contents.Update(content);
                }
                else
                {
                    content.ContentCategories.Remove(contentCategory);
                    _context.ContentCategories.Remove(contentCategory);
                }
            }

            await _context.SaveChangesAsync();

            return category.Id;
        }
    }
}
