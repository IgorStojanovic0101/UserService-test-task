using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Application.DTOs.User;
using Test.Application.Utilities;

namespace Test.Application.Validators
{
    public class UpdateUserRoleValidator : AbstractValidator<UserUpdateDTO>
    {
        public UpdateUserRoleValidator()
        {
            RuleFor(dto => dto.Role)
                .Must(role => role == Roles.Admin || role == Roles.User)
                .WithMessage("Invalid role.");
        }
    }
}
