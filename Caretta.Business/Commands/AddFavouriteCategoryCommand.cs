using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caretta.Business.Commands
{
    public class AddFavouriteCategoryCommand : IRequest<Guid>
    {
        public Guid CategoryId { get; set; }

        public AddFavouriteCategoryCommand(Guid categoryId)
        {
            CategoryId = categoryId;
        }
    }
}
