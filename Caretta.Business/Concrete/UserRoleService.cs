using AutoMapper;
using Caretta.Business.Abstract;
using Caretta.Core.Dto;
using Caretta.Core.Entity;
using Caretta.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Caretta.Core.Exceptions;

namespace Caretta.Business.Concrete
{
    public class UserRoleService : IUserRoleInterface
    {
        private readonly CarettaContext _context;
        private readonly IMapper _mapper;

        public UserRoleService(CarettaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserRoleDto>> GetAllUserRolesAsync()
        {
            var userRoles = await _context.UserRoles.ToListAsync();
            return _mapper.Map<IEnumerable<UserRoleDto>>(userRoles);
        }

        public async Task AssignUserRoleAsync(UserRoleDto userRoleDto)
        {
            var userRole = _mapper.Map<UserRole>(userRoleDto);
            _context.UserRoles.Add(userRole);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveUserRoleAsync(Guid userId, Guid roleId)
        {
            var userRole = await _context.UserRoles
                .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);

            if (userRole != null)
            {
                _context.UserRoles.Remove(userRole);
                await _context.SaveChangesAsync();
            }
        }
    }
}
