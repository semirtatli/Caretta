using Caretta.Core.Dto.UserDto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caretta.Business.Validation
{
    public class UserCreateDtoValidator : AbstractValidator<UserCreateDto>
    {
        public UserCreateDtoValidator()
        {

            //
            RuleFor(x => x.Name)
           .NotEmpty().WithMessage("Name is required.")
           .Length(2, 50).WithMessage("Name must be between 2 and 50 characters.")
           .Matches("^[a-zA-Z ]*$").WithMessage("Name must not contain numbers or special characters.");

            RuleFor(x => x.SurName)
                .NotEmpty().WithMessage("Surname is required.")
                .Length(2, 25).WithMessage("Surname must be between 2 and 25 characters.")
                .Matches("^[a-zA-Z ]*$").WithMessage("Surname must not contain numbers or special characters.");

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username is required.")
                .Length(3, 40).WithMessage("Username must be between 3 and 40 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.");
                //.EmailAddress().WithMessage("Invalid email format.");
            /*
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .Length(6, 30).WithMessage("Password must be between 6 and 30 characters long.")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches("[0-9]").WithMessage("Password must contain at least one number.");
            */
            //.Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");

            RuleFor(x => x.TC)
                .NotEmpty().WithMessage("TC is required.")
                .Length(11).WithMessage("TC must be exactly 11 characters long.")
                .Matches("^[0-9]*$").WithMessage("TC must only contain numbers.");
            //.Must(BeAValidTC).WithMessage("TC is not valid.");

            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required.")
                .Length(2, 50).WithMessage("Full name must be between 2 and 50 characters.")
                .Matches("^[a-zA-Z ]*$").WithMessage("Full name must not contain numbers or special characters.");
            /*
             RuleFor(x => x.UserRoles)
                     .NotEmpty().WithMessage("There must be at least one User Role.");
 */
        }

        private bool BeAValidTC(string tc)
        {
            if (tc.Length != 11) return false;
            if (tc[0] == '0') return false;

            int[] digits = tc.Select(t => int.Parse(t.ToString())).ToArray();

            int oddSum = digits[0] + digits[2] + digits[4] + digits[6] + digits[8];
            int evenSum = digits[1] + digits[3] + digits[5] + digits[7];

            int digit10 = (oddSum * 7 - evenSum) % 10;
            int digit11 = (oddSum + evenSum + digits[9]) % 10;

            return digits[9] == digit10 && digits[10] == digit11;
        }
    }
}
