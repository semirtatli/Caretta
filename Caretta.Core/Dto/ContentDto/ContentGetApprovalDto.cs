using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caretta.Core.Dto.ContentDto
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Caretta.Core.BaseEntity;
    using Caretta.Core.Entity;
    using Caretta.Core.Enums;
    using Caretta.Core.Dto.CategoryDto;


        public class ContentGetApprovalDto
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

        public string RejectReason { get; set; }
        }

}
