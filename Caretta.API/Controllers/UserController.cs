using Microsoft.AspNetCore.Mvc;
using Caretta.Business.Abstract;
using Caretta.Core.Dto.UserDto;
using Caretta.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Caretta.Business.Concrete;
using Caretta.Core.Entity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;

namespace Caretta.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        public static User user = new User();
        private readonly IUserInterface _userService;

        public UsersController(IUserInterface userService)
        {
            _userService = userService;
        }


        [HttpGet]
        [Route("GetAllUsers"), Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<UserGetDto>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet]
        [Route("GetUserById"), Authorize(Roles = "admin")]
        public async Task<ActionResult<UserGetDto>> GetUserById(Guid id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                return Ok(user);
            }
            catch (NotFoundException)
            {
                return NotFound("User not found.");
            }
        }

        [HttpPost]
        [Route("CreateUser"), Authorize(Roles = "admin")]
        public async Task<ActionResult<UserCreateDto>> CreateUser([FromBody] UserCreateDto userDto)
        {
            var createdUser = await _userService.CreateOrUpdateUserAsync(null, userDto);
            return Ok(createdUser);
        }

        [HttpPut]
        [Route("UpdateUser"), Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UserCreateDto userDto)
        {
            try
            {
                await _userService.CreateOrUpdateUserAsync(id, userDto);
                return NoContent();
            }
            catch (NotFoundException)
            {
                return NotFound($"User with ID {id} not found.");
            }
        }


        [HttpDelete]
        [Route("DeleteUser"), Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            try
            {
                await _userService.DeleteUserAsync(id);
                return NoContent();
            }
            catch (NotFoundException)
            {
                return NotFound($"User with ID {id} not found.");
            }
        }
        



    }
}
