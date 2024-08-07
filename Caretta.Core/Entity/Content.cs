using Caretta.Core.BaseEntity;
using Caretta.Core.Dto;
using Caretta.Core.Dto.CategoryDto;
using Caretta.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caretta.Core.Entity
{
    public class Content : AuditableEntityBase
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public ContentType ContentType { get; set; }
        public ContentStatus ContentStatus { get; set; } = 0;
        public DateTime PublishDate { get; set; }
        public int CommentCount { get; set; } = 0;
        public int LikesCount { get; set; }
        public bool IsApproved { get; set; } = false;
        public string? RejectReason { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; } = ApprovalStatus.Draft;
        public string ApprovalStatusDescription { get; set; }


        public ICollection<ContentCategories> ContentCategories { get; set; }

        public ICollection<UserLikeContents> UserLikeContents { get; set; }
        //public ContentCategoryDto ContentCategories { get; set; }
        //public ContentType ContentTypeId { get; set; }
        //public ICollection<CategoryGetDto> Categories { get; set; }
        //public Guid ContentCategoryId { get; set; }
        //public CategoryGetDto Category { get; set; }
        public ICollection<Comment> Comments { get; set; }

    }
}
