using AutoMapper;
using Caretta.Business.Abstract;
using Caretta.Core.Entity;
using Caretta.Core.Exceptions;
using Caretta.Core.Dto.RoleDto;
using Caretta.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Caretta.Business.Concrete
{
    public class RoleService : IRoleInterface
    {
        private readonly CarettaContext _context;
        private readonly IMapper _mapper;
        private readonly UserContextService _userContextService;

        public RoleService(CarettaContext context, IMapper mapper, UserContextService userContextService)
        {
            _context = context;
            _mapper = mapper;
            _userContextService = userContextService;
        }

        public async Task<IEnumerable<RoleGetDto>> GetAllRolesAsync()
        {
            var roles = await _context.Roles
                .Where(r => !r.IsDeleted)
                .ToListAsync();

            return _mapper.Map<IEnumerable<RoleGetDto>>(roles);
        }

        public async Task<RoleGetByIdDto> GetRoleByIdAsync(Guid roleId)
        {
            var role = await _context.Roles
                .Where(r => r.Id == roleId)
                .Include(r => r.UserRoles)
                .ThenInclude(rr => rr.User)
                .FirstOrDefaultAsync();

            if (role == null)
                throw new NotFoundException("Role not found.");
            if (role.IsDeleted)
            {
                throw new DeletedException("Role is deleted.");
            }
            return _mapper.Map<RoleGetByIdDto>(role);
        }

        public async Task<RoleCreateDto> CreateOrUpdateRoleAsync(Guid? roleId, RoleCreateDto roleDto)
        {
            Role role;

            if (roleId.HasValue)
            {
                role = await _context.Roles
                    .FirstOrDefaultAsync(r => r.Id == roleId.Value && !r.IsDeleted);

                if (role == null)
                    throw new NotFoundException("Role to be updated not found.");
                if (role.IsDeleted)
                    throw new DeletedException("Role is deleted.");

                _mapper.Map(roleDto, role);
                role.ModifiedOn = DateTime.UtcNow;
                role.ModifiedBy = _userContextService.GetCurrentUserId();
                _context.Roles.Update(role);
            }
            else
            {
                role = _mapper.Map<Role>(roleDto);
                role.CreatedOn = DateTime.UtcNow;
                role.CreatedBy = _userContextService.GetCurrentUserId();
                _context.Roles.Add(role);
            }

            await _context.SaveChangesAsync();
            return _mapper.Map<RoleCreateDto>(role);
        }
        /*

        public async Task<RoleCreateDto> CreateRoleAsync(RoleCreateDto roleDto)
        {
            var role = _mapper.Map<Role>(roleDto);
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();

            return _mapper.Map<RoleCreateDto>(role);
        }

        public async Task UpdateRoleAsync(Guid roleId, RoleCreateDto roleDto)
        {
            var role = await _context.Roles
                .FirstOrDefaultAsync(r => r.Id == roleId && !r.IsDeleted);

            if (role == null)
                throw new NotFoundException("Role to be updated not found.");
            if (role.IsDeleted)
            {
                throw new DeletedException("Role is deleted.");
            }
            _mapper.Map(roleDto, role);
            role.ModifiedOn = DateTime.UtcNow;

            _context.Roles.Update(role);
            await _context.SaveChangesAsync();
        }
        */
        public async Task DeleteRoleAsync(Guid roleId)
        {
            var role = await _context.Roles
                .FirstOrDefaultAsync(r => r.Id == roleId);

            if (role == null)
                throw new NotFoundException("Role to be deleted not found.");
            if (role.IsDeleted)
            {
                throw new DeletedException("Role is deleted.");
            }
            role.IsDeleted = true;

            _context.Roles.Update(role);
            await _context.SaveChangesAsync();
        }
    }
}
