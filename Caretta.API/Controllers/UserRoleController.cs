using Microsoft.AspNetCore.Mvc;
using Caretta.Business.Abstract;
using Caretta.Core.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Caretta.API.Controllers
{
    [ApiController]
    [Route("api/userRoles")]
    public class UserRolesController : ControllerBase
    {
        private readonly IUserRoleInterface _userRoleService;

        public UserRolesController(IUserRoleInterface userRoleService)
        {
            _userRoleService = userRoleService;
        }

        [HttpGet("GetAllUserRoles")]
        public async Task<ActionResult<IEnumerable<UserRoleDto>>> GetAllUserRoles()
        {
            var userRoles = await _userRoleService.GetAllUserRolesAsync();
            return Ok(userRoles);
        }

        [HttpPost("AssignUserRole")]
        public async Task<ActionResult> AssignUserRole([FromBody] UserRoleDto userRoleDto)
        {
            await _userRoleService.AssignUserRoleAsync(userRoleDto);
            return NoContent();
        }

        [HttpDelete("RemoveUserRole")]
        public async Task<ActionResult> RemoveUserRole(Guid userId, Guid roleId)
        {
            await _userRoleService.RemoveUserRoleAsync(userId, roleId);
            return NoContent();
        }
    }
}
