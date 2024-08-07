using Caretta.Core.BaseEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caretta.Core.Dto.UserDto
{
    public class UserCreateDto
    {
        public string Name { get; set; }
        public string SurName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string TC { get; set; }
        public ICollection<Guid> RoleIds { get; set; }
        public string FullName { get; set; }

        // public ICollection<RoleDto> Roles { get; set; }

    }
}
