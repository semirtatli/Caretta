using Microsoft.AspNetCore.Mvc;
using Caretta.Business.Abstract;
using Caretta.Core.Dto.RoleDto;
using Caretta.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Caretta.Business.Concrete;
using Microsoft.AspNetCore.Authorization;

namespace Caretta.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleInterface _roleService;

        public RolesController(IRoleInterface roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        [Route("GetAllRoles"), Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<RoleGetDto>>> GetAllRoles()
        {
            var roles = await _roleService.GetAllRolesAsync();
            return Ok(roles);
        }

        [HttpGet]
        [Route("GetRoleById"), Authorize(Roles = "admin")]
        public async Task<ActionResult<RoleGetByIdDto>> GetRoleById(Guid id)
        {
            try
            {
                var role = await _roleService.GetRoleByIdAsync(id);
                return Ok(role);
            }
            catch (NotFoundException)
            {
                return NotFound("Role not found.");
            }
        }

        [HttpPost]
        [Route("CreateRole"), Authorize(Roles = "admin")]
        public async Task<ActionResult<RoleGetDto>> CreateRole([FromBody] RoleCreateDto roleDto)
        {
            var createdRole = await _roleService.CreateOrUpdateRoleAsync(null, roleDto);
            return Ok();
        }

        [HttpPut]
        [Route("UpdateRole"), Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateRole(Guid id, [FromBody] RoleCreateDto roleDto)
        {
            try
            {
                await _roleService.CreateOrUpdateRoleAsync(id, roleDto);
                return NoContent();
            }
            catch (NotFoundException)
            {
                return NotFound($"Role with ID {id} not found.");
            }
        }

        [HttpDelete]
        [Route("DeleteRole"), Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteRole(Guid id)
        {
            try
            {
                await _roleService.DeleteRoleAsync(id);
                return NoContent();
            }
            catch (NotFoundException)
            {
                return NotFound($"Role with ID {id} not found.");
            }
        }
    }
}
