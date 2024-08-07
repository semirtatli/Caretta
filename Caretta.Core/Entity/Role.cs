using Caretta.Core.BaseEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caretta.Core.Entity
{
    public class Role : AuditableEntityBase
    {
        public string RoleType { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }
    }
}
