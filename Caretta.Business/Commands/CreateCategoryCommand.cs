using Caretta.Core.Dto.CategoryDto;
using MediatR;

namespace Caretta.Business.Commands
{
    public class CreateCategoryCommand : IRequest<Guid>
    {
        public CategoryCreateDto CategoryDto { get; set; }

        public CreateCategoryCommand(CategoryCreateDto categoryDto)
        {
            CategoryDto = categoryDto;
        }
    }
}
