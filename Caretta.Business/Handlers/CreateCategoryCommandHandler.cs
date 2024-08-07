using AutoMapper;
using Caretta.Business.Commands;
using Caretta.Business.Concrete;
using Caretta.Core.Entity;
using Caretta.Data.Context;
using MediatR;
using System;
using System.Reflection.Metadata;
using System.Threading;
using System.Threading.Tasks;

namespace Caretta.Business.Handlers
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Guid>
    {
        private readonly CarettaContext _context;
        private readonly IMapper _mapper;
        private readonly UserContextService _userContextService;

        public CreateCategoryCommandHandler(CarettaContext context, IMapper mapper, UserContextService userContextService)
        {
            _context = context;
            _mapper = mapper;
            _userContextService = userContextService;
        }

        public async Task<Guid> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = _mapper.Map<Category>(request.CategoryDto);
            category.CreatedOn = DateTime.UtcNow;
            category.CreatedBy = _userContextService.GetCurrentUserId();

            _context.Categories.Add(category);
            await _context.SaveChangesAsync(cancellationToken);

            

            return category.Id;
        }
    }
}
