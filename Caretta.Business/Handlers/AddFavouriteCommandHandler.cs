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
    public class AddFavouriteCategoryCommandHandler : IRequestHandler<AddFavouriteCategoryCommand, Guid>
    {
        private readonly CarettaContext _context;
        private readonly IMapper _mapper;
        private readonly UserContextService _userContextService;

        public AddFavouriteCategoryCommandHandler(CarettaContext context, IMapper mapper, UserContextService userContextService)
        {
            _context = context;
            _mapper = mapper;
            _userContextService = userContextService;
        }

        public async Task<Guid> Handle(AddFavouriteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == request.CategoryId);
            if (category == null)
                throw new NotFoundException("Category not found.");

            if (request.CategoryId != null)
            {
                //foreach (categoryId)
                {
                    var userFavouriteCategories = new UserFavouriteCategories
                    {
                        UserId = _userContextService.GetCurrentUserId(),
                        CategoryId = request.CategoryId
                    };
                    _context.UserFavouriteCategories.Add(userFavouriteCategories);
                }
            }

            await _context.SaveChangesAsync();

            return request.CategoryId;
        }
    }

}
