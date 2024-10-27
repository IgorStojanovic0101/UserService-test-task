using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Application.DTOs.User
{
    public record UserNotifyUpdateDTO
    (
        string Email,
        string Message
    );
}
