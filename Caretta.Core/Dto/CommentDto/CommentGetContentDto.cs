using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caretta.Core.Dto.CommentDto
{
    public class CommentGetContentDto
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public int LikesCount { get; set; }
        public bool IsApproved { get; set; }
    }
}
