using Caretta.Core.Dto.CategoryDto;
using Caretta.Core.Dto.ContentDto;
using MediatR;

namespace Caretta.Business.ContentDAL.Commands
{
    public class CreateContentCommand : IRequest<Guid>
    {
        public ContentCreateDto ContentDto { get; set; }

        public CreateContentCommand(ContentCreateDto contentDto)
        {
            ContentDto = contentDto;
        }
    }
}
