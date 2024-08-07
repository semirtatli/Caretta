using Caretta.Core.BaseEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caretta.Core.Entity
{
    public class User : AuditableEntityBase
    {
        public string Name { get; set; }
        public string SurName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string TC {  get; set; }
        public string FullName { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }
        //public ICollection<Category> FavoriteCategories { get; set; }
        public ICollection<UserFavouriteCategories> UserFavouriteCategories { get; set; }

        public ICollection<UserLikeContents> UserLikeContents { get; set; }
        public ICollection<UserLikeComments> UserLikeComments { get; set; }

    }
}
