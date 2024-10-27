using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Application.DTOs.User
{
    public record UserCreationDTO
    (
         string Name,
         string Password,
         string Email,
         string Role
    );
}
