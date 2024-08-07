using Caretta.Core.Dto.RoleDto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caretta.Business.Validation
{
    public class RoleCreateDtoValidator : AbstractValidator<RoleCreateDto>
    {
        public RoleCreateDtoValidator()
        {
            //
            RuleFor(x => x.RoleType)
                .NotEmpty().WithMessage("Role type is required.")
                .Length(1, 50).WithMessage("Role type must be between 1 and 50 characters.")
                .Matches("^[a-zA-Z ]*$").WithMessage("Role Type must not contain numbers or special characters.");
            //
            /*
            RuleFor(x => x.UserRoles)
                    .NotEmpty().WithMessage("There must be at least one User Role.");*/
        }
    }
}
