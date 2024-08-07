using Caretta.Core.Dto.CategoryDto;
using Caretta.Core.Dto.RoleDto;
using Caretta.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caretta.Core.Dto.UserDto
{
    public class UserGetDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public ICollection<RoleGetDto> Roles { get; set; }
        public string FullName { get; set; }
        //burda mi yeni dto mu
        public ICollection<CategoryGetDto> FavoriteCategories { get; set; }
        // public ICollection<RoleDto> Roles { get; set; }

    }
}
