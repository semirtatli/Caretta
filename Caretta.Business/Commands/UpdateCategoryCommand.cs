using Caretta.Core.Dto.CategoryDto;
using MediatR;
using System;

namespace Caretta.Business.Commands
{
    public class UpdateCategoryCommand : IRequest<Guid>
    {
        public Guid CategoryId { get; set; }
        public CategoryCreateDto CategoryDto { get; set; }

        public UpdateCategoryCommand(Guid categoryId, CategoryCreateDto categoryDto)
        {
            CategoryId = categoryId;
            CategoryDto = categoryDto;
        }
    }
}
