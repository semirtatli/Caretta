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

namespace Caretta.Business.Concrete
{
    public class UserService : IUserInterface
    {
        private readonly CarettaContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration; 
        private readonly UserContextService _userContextService;
        private readonly ICategoryInterface _categoryService;
        private readonly IContentInterface _contentService;
        private readonly ICommentInterface _commentService;
        private Guid DefaultRoleId = new Guid("3FA85F64-5717-4562-B3FC-2C963F66AFE1");

        public UserService(CarettaContext context, IMapper mapper, IConfiguration configuration, UserContextService userContextService, ICategoryInterface categoryService, IContentInterface contentService, ICommentInterface commentService)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
            _userContextService = userContextService;
            _categoryService = categoryService;
            _contentService = contentService;
            _commentService = commentService;
        }

        public async Task<IEnumerable<UserGetDto>> GetAllUsersAsync()
        {
            var users = await _context.Users
                .Include(c => c.UserRoles)
                .ThenInclude(cc => cc.Role)
                .Where(u => !u.IsDeleted)
                .ToListAsync();

            return _mapper.Map<IEnumerable<UserGetDto>>(users);
        }

        public async Task<UserGetDto> GetUserByIdAsync(Guid userId)
        {
            var user = await _context.Users
                .Include(c => c.UserRoles)
                .ThenInclude(cc => cc.Role)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new NotFoundException("User not found.");
            if (user.IsDeleted)
            {
                throw new DeletedException("User is deleted.");
            }
            return _mapper.Map<UserGetDto>(user);
        }
        public async Task<UserCreateDto> CreateOrUpdateUserAsync(Guid? userId, UserCreateDto userDto)
        {
            User user;

            if (userId.HasValue)
            {
                user = await _context.Users
                    .Include(u => u.UserRoles)
                    .FirstOrDefaultAsync(u => u.Id == userId.Value);

                if (user == null)
                    throw new NotFoundException("User to be updated not found.");
                if (user.IsDeleted)
                    throw new DeletedException("User is deleted.");

                _mapper.Map(userDto, user);
                user.ModifiedOn = DateTime.UtcNow;
                user.ModifiedBy = _userContextService.GetCurrentUserId();
                // Remove existing roles
                var existingRoles = _context.UserRoles.Where(ur => ur.UserId == userId.Value).ToList();
                _context.UserRoles.RemoveRange(existingRoles);

                CreatePasswordHash(userDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

                
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;

            }
            else
            {
                CreatePasswordHash(userDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

                user = _mapper.Map<User>(userDto);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;

                user.CreatedOn = DateTime.UtcNow;
                user.CreatedBy = _userContextService.GetCurrentUserId();
                _context.Users.Add(user);
            }

            await _context.SaveChangesAsync();

            if (userDto.RoleIds != null && userDto.RoleIds.Any())
            {
                foreach (var roleId in userDto.RoleIds)
                {
                    var userRole = new UserRole
                    {
                        UserId = user.Id,
                        RoleId = roleId
                    };
                    _context.UserRoles.Add(userRole);
                }
            }

            await _context.SaveChangesAsync();

            return _mapper.Map<UserCreateDto>(user);
        }



        /*
        public async Task<UserCreateDto> CreateUserAsync(UserCreateDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return _mapper.Map<UserCreateDto>(user);
        }

        public async Task UpdateUserAsync(Guid userId, UserCreateDto userDto)
        {
            var user = await _context.Users
                .Include(c => c.UserRoles)
                .ThenInclude(cc => cc.Role)
                .FirstOrDefaultAsync(u => u.Id == userId && !u.IsDeleted);

            if (user == null)
                throw new NotFoundException("User to be updated not found.");
            if (user.IsDeleted)
            {
                throw new DeletedException("User is deleted.");
            }
            _mapper.Map(userDto, user);
            user.ModifiedOn = DateTime.UtcNow;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
        */
        public async Task DeleteUserAsync(Guid userId)
        {
            var user = await _context.Users
                .Include(c => c.UserRoles)
                .ThenInclude(cc => cc.Role)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new NotFoundException("User to be deleted not found.");
            if (user.IsDeleted)
            {
                throw new DeletedException("User is deleted.");
            }

            // Delete related categories, contents, and comments
            var categories = await _context.Categories
                .Where(c => c.CreatedBy == userId && !c.IsDeleted)
                .ToListAsync();

            foreach (var category in categories)
            {
                await _categoryService.DeleteCategoryAsync(category.Id);
            }

            var contents = await _context.Contents
                .Where(c => c.CreatedBy == userId && !c.IsDeleted)
                .ToListAsync();

            foreach (var content in contents)
            {
                await _contentService.DeleteContentAsync(content.Id);
            }

            var comments = await _context.Comments
                .Where(c => c.CreatedBy == userId && !c.IsDeleted)
                .ToListAsync();

            foreach (var comment in comments)
            {
                await _commentService.DeleteCommentAsync(comment.Id);
            }


            user.IsDeleted = true;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        //
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        

    }
}
