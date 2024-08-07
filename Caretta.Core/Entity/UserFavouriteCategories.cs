using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caretta.Core.Entity
{
    public class UserFavouriteCategories
    {
        public Guid UserId { get; set; }
        public Guid CategoryId { get; set; }
        public User User { get; set; }
        public Category Category { get; set; }
    }
}
