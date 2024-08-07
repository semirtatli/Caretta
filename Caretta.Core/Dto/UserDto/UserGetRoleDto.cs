﻿using Caretta.Core.Dto.RoleDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caretta.Core.Dto.UserDto
{
    public class UserGetRoleDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
 
        public string FullName { get; set; }

    }
}
