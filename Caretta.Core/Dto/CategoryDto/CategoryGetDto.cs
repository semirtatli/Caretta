using Caretta.Core.BaseEntity;
using Caretta.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;

namespace Caretta.Core.Dto.CategoryDto
{

    public class CategoryGetDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        //public ICollection<ContentDto> Contents { get; set; }

    }

}
