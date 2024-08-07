using Caretta.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caretta.Core.Dto.UserDto;
using Microsoft.Extensions.Configuration;

namespace Caretta.Business.Abstract
{
    public interface IUserInterface
    {
        Task<IEnumerable<UserGetDto>> GetAllUsersAsync();
        Task<UserGetDto> GetUserByIdAsync(Guid userId);
        Task<UserCreateDto> CreateOrUpdateUserAsync(Guid? userId, UserCreateDto userDto);

        
        Task DeleteUserAsync(Guid userId);
    }
}
