using Caretta.Core.Entity;
using Caretta.Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caretta.Business.Abstract
{
    public interface IUserRoleInterface
    {
        //?
        Task<IEnumerable<UserRoleDto>> GetAllUserRolesAsync();
        Task AssignUserRoleAsync(UserRoleDto userRole);
        Task RemoveUserRoleAsync(Guid userId, Guid roleId);
    }
}

