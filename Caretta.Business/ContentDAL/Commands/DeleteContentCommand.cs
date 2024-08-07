using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caretta.Business.ContentDAL.Commands
{
    public class DeleteContentCommand : IRequest<Guid>
    {
        public Guid ContentId { get; set; }

        public DeleteContentCommand(Guid contentId)
        {
            ContentId = contentId;
        }
    }
}
