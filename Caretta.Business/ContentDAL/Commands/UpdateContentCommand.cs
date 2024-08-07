using Caretta.Core.Dto.CategoryDto;
using Caretta.Core.Dto.ContentDto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caretta.Business.ContentDAL.Commands
{
    public class UpdateContentCommand : IRequest<Guid>
    {
        public Guid ContentId { get; set; }
        public ContentCreateDto ContentDto { get; set; }

        public UpdateContentCommand(Guid contentId, ContentCreateDto categoryDto)
        {
            ContentId = contentId;
            ContentDto = categoryDto;
        }
    }
}
