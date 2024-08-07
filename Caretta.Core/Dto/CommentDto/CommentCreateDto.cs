using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caretta.Core.Dto.CommentDto
{
    public class CommentCreateDto
    {
        public string Text { get; set; }

        public Guid ContentId { get; set; }
        // public ContentDto Content { get; set; }
    }
}
