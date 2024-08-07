using Caretta.Core.Dto.CategoryDto;
using Caretta.Core.Dto.CommentDto;
using Caretta.Core.Entity;
using Caretta.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caretta.Core.Dto.ContentDto
{
    public class ContentGetByIdDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int LikesCount { get; set; }
        public ContentType ContentType { get; set; }
        public ContentStatus ContentStatus { get; set; }
        public DateTime PublishDate { get; set; }
        public int CommentCount { get; set; }
        public ICollection<CommentGetContentDto> Comments { get; set; }
        public bool IsApproved { get; set; }

        public ICollection<CategoryGetDto> Categories { get; set; }

    }
}
