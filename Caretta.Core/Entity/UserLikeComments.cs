using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caretta.Core.Entity
{
    public class UserLikeComments
    {
        public Guid UserId { get; set; }
        public Guid CommentId { get; set; }
        public DateTime LikeDate { get; set; }
        public User User { get; set; }
        public Comment Comment { get; set; }
    }
}
