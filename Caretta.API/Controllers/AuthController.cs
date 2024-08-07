using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Caretta.Core.Entity;
using Caretta.Core.Dto.UserDto;
using Caretta.Business.Concrete;
using Caretta.Business.Abstract;
using AutoMapper;

namespace Caretta.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static User user = new User();
        private readonly IAuthInterface _authService;
        private readonly IUserInterface _userService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public AuthController(IConfiguration configuration, IAuthInterface authService, IMapper mapper, IUserInterface userService)

        {
            _mapper = mapper;
            _authService = authService;
            _configuration = configuration;
            _userService = userService;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<User>> Register(SignUpDto request)
        {

            var createdUser = await _authService.RegisterAsync(request);
            user = _mapper.Map<User>(createdUser);
            return Ok(createdUser);

        }

        [HttpPost("Login")]
        public async Task<ActionResult<string>> Login(LoginDto request)
        {
            var user = await _authService.LoginAsync(request);
            if (user == null)
            {
                return BadRequest("User not found.");
            }
            if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Wrong Password.");
            }
            string token = _authService.CreateToken(user);
            return Ok(token);
        }

        [HttpPost("ForgotPassword")]
        public async Task<ActionResult<string>> ForgotPassword(ForgotPasswordDto request)
        {
            var user = await _authService.ForgotPasswordAsync(request);
            if (user == null)
            {
                return BadRequest("User not found.");
            }
            
            
            return Ok();

        }



        /*
        private string CreateToken(User user)
        {


            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, "User")
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
        */

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}
