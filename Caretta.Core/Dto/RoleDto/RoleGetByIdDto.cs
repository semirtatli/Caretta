using Caretta.Core.Dto.ContentDto;
using Caretta.Core.Dto.UserDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caretta.Core.Dto.RoleDto
{
    public class RoleGetByIdDto
    {

        public Guid Id { get; set; }
        public string RoleType { get; set; }
        public ICollection<UserGetRoleDto> Users { get; set; }

    }
}
