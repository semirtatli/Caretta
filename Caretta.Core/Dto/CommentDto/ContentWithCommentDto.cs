using Caretta.Core.Dto.ContentDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caretta.Core.Dto.CommentDto
{
    public class ContentWithCommentsDto
    {
        public ContentGetDto Content { get; set; }
        public IEnumerable<CommentGetDto> Comments { get; set; }
    }
}
