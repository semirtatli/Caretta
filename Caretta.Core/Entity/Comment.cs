using Caretta.Core.BaseEntity;
using Caretta.Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caretta.Core.Entity
{
    public class Comment : AuditableEntityBase
    {
        public Guid ContentId { get; set; }
        public string Text { get; set; }
        public Content Content { get; set; }
        public int LikesCount { get; set; }
        public bool IsApproved { get; set; } = false;

        public ICollection<UserLikeComments> UserLikeComments { get; set; }
    }
}
