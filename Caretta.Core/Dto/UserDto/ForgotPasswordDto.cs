using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caretta.Core.Dto.UserDto
{
    public class ForgotPasswordDto
    {
        public string TC { get; set; }
        public string UserName { get; set; }
        public string newPassword { get; set; }
    }
}
