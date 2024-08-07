using Caretta.Core.Dto.UserDto;
using Caretta.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caretta.Business.Abstract
{
    public interface IAuthInterface
    {
        Task<UserGetDto> RegisterAsync(SignUpDto userDto);
        Task<User> LoginAsync(LoginDto userDto);

        Task<User> ForgotPasswordAsync(ForgotPasswordDto userPassword);

        string CreateToken(User user);
    }
}
