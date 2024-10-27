using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Application.DTOs.User;
using Test.Domain.Entities;

namespace Test.Application.Abstraction
{
    public interface IUserService
    {
        Task<string> CreateUserAsync(string name, string email, string password, string role);
        Task<IEnumerable<UserModel>> GetUsersAsync();
        Task<string> UpdateUserRoleAsync(int userId, string newRole);

    }
}
