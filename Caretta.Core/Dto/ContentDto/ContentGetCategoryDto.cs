using Caretta.Core.Dto.CategoryDto;
using Caretta.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caretta.Core.Dto.ContentDto
{
    public class ContentGetCategoryDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int LikesCount { get; set; }
        public int CommentCount { get; set; }
        public ContentType ContentType { get; set; }
    }
}
