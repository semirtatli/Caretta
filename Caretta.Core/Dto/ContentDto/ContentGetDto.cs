using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caretta.Core.BaseEntity;
using Caretta.Core.Entity;
using Caretta.Core.Enums;
using Caretta.Core.Dto.CategoryDto;


namespace Caretta.Core.Dto.ContentDto
{
    public class ContentGetDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int LikesCount { get; set; }
        public ContentType ContentType { get; set; }
        public int CommentCount { get; set; }
        public ContentStatus ContentStatus { get; set; }
        public DateTime PublishDate { get; set; }
        public ICollection<CategoryGetDto> Categories { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public string ApprovalStatusDescription { get; set; }


        //public ICollection<string> Categories { get; set; }
        // public ICollection<CommentDto> Comments { get; set; }
        //public ContentTypeDto ContentType { get; set; }
    }
}