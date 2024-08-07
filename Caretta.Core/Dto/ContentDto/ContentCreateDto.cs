using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caretta.Core.BaseEntity;
using Caretta.Core.Entity;
using Caretta.Core.Dto.CategoryDto;
using Caretta.Core.Enums;

namespace Caretta.Core.Dto.ContentDto
{
    public class ContentCreateDto
    {
        public string Title { get; set; }

        public string Body { get; set; }
        public ContentType ContentType { get; set; }
        public DateTime PublishDate { get; set; }
        public ICollection<Guid> CategoryIds { get; set; }
        //public ContentCategoryDto ContentCategory { get; set; }
        // public ICollection<CommentDto> Comments { get; set; }
        //  public ContentTypeDto ContentType { get; set; }
    }
}
