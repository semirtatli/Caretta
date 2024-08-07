using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caretta.Core.Dto
{
    public class UserRoleDto
    {
        public ICollection<Guid> RoleIds { get; set; }
    }
}
