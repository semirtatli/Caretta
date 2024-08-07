using Caretta.Core.BaseEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;

namespace Caretta.Core.Entity
{
    public class Category : AuditableEntityBase
    {
        public string Name { get; set; }
        public ICollection<ContentCategories> ContentCategories { get; set; }

        public ICollection<UserFavouriteCategories> UserFavouriteCategories { get; set; }
    }
    
}
