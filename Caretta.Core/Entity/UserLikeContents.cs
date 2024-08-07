using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caretta.Core.Entity
{
    public class UserLikeContents
    {
        public Guid UserId { get; set; }
        public Guid ContentId { get; set; }
        public DateTime LikeDate { get; set; }
        public User User { get; set; }
        public Content Content { get; set; }
    }
}
