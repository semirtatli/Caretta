using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caretta.Core.Dto
{
    public class ContentCategoryDto
    {
        public ICollection<Guid> CategoryIds { get; set; }
    }
}
