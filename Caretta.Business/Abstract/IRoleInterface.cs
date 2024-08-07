using Caretta.Core.Entity;
using Caretta.Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caretta.Core.Dto.RoleDto;

namespace Caretta.Business.Abstract
{
    public interface IRoleInterface
    {
        Task<IEnumerable<RoleGetDto>> GetAllRolesAsync();
        Task<RoleGetByIdDto> GetRoleByIdAsync(Guid roleId);
        Task<RoleCreateDto> CreateOrUpdateRoleAsync(Guid? roleId, RoleCreateDto roleDto);
        /*Task<RoleCreateDto> CreateRoleAsync(RoleCreateDto roleDto);
        Task UpdateRoleAsync(Guid roleId, RoleCreateDto roleDto);*/
        Task DeleteRoleAsync(Guid roleId);
    }
}