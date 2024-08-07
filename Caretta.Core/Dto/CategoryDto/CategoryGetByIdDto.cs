using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caretta.Core.Dto.ContentDto;
namespace Caretta.Core.Dto.CategoryDto
{
    public class CategoryGetByIdDto

    
    {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public ICollection<ContentGetCategoryDto> Contents { get; set; }
    }
}

