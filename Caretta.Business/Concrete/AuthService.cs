using AutoMapper;
using Caretta.Business.Abstract;
using Caretta.Core.Entity;
using Caretta.Core.Exceptions;
using Caretta.Core.Dto.UserDto;
using Caretta.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Org.BouncyCastle.Asn1.Ocsp;

namespace Caretta.Business.Concrete
{
    public class AuthService : IAuthInterface
    {
        private readonly CarettaContext _context;
        private readonly IMapper _mapper;
        private readonly IUserInterface _userService;
        private readonly IConfiguration _configuration;
        private Guid DefaultRoleId = new Guid("3FA85F64-5717-4562-B3FC-2C963F66AFE1");

        public AuthService(CarettaContext context, IMapper mapper, IConfiguration configuration, IUserInterface userService)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
            _userService = userService;
        }

        public async Task<UserGetDto> RegisterAsync(SignUpDto userDto)
        {
            if (await _context.Users.AnyAsync(u => u.UserName == userDto.UserName))
            {
                throw new Exception("User with the same username already exists.");
            }

            CreatePasswordHash(userDto.Password, out byte[] passwordHash, out byte[] passwordSalt);
            var user = _mapper.Map<User>(userDto);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            // Add the user to the context and save changes to generate the UserId
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Now that the UserId is generated, create the UserRole entity
            var userRole = new UserRole
            {
                UserId = user.Id,  // Use the generated UserId
                RoleId = DefaultRoleId
            };

            // Add the UserRole entity to the context
            _context.UserRoles.Add(userRole);
            await _context.SaveChangesAsync();

            // Retrieve the created user with roles included
            var createdUser = await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            return _mapper.Map<UserGetDto>(createdUser);
        }

        public async Task<User> LoginAsync(LoginDto userDto)
        {

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == userDto.UserName && u.IsDeleted == false);

            return user;
        }

        public async Task<User> ForgotPasswordAsync(ForgotPasswordDto request)
        {
            var user = await _context.Users
                .Include(c => c.UserRoles)
                .ThenInclude(cc => cc.Role)
                .FirstOrDefaultAsync(u => u.UserName == request.UserName);

            if (user == null)
                throw new NotFoundException("User not found.");
            if (user.IsDeleted)
            {
                throw new DeletedException("User is deleted.");
            }
            if (user.UserName == request.UserName && user.TC == request.TC)
            {

                var userToUpdate = _mapper.Map<UserCreateDto>(user);
                userToUpdate.Password = request.newPassword;
                Guid userToUpdateId = user.Id;
                await _userService.CreateOrUpdateUserAsync(userToUpdateId, userToUpdate);
            }
            return user;
        }


        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public string CreateToken(User user)
        {
            var roles = _context.UserRoles
                .Where(ur => ur.UserId == user.Id)
                .Include(ur => ur.Role)
                .Select(ur => ur.Role.RoleType)
                .ToList();

            string tokenType = "user";

            foreach (var role in roles)
            {
                
                if (role.Equals("admin", StringComparison.OrdinalIgnoreCase))
                {
                    tokenType = "admin";
                }
                else if (role.Equals("editor", StringComparison.OrdinalIgnoreCase))
                {
                    tokenType = "editor";
                }
                else if (role.Equals("writer", StringComparison.OrdinalIgnoreCase))
                {
                    tokenType = "writer";
                }
            }

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                //new Claim(ClaimTypes.Role, tokenType),
                new Claim(ClaimTypes.Role, "user")
            };

            roles.ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role)));

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

    }
}
