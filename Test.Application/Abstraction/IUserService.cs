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

        Task<(bool IsValid, List<string> Errors)> CreateUserValidator(UserCreationDTO dto);
        Task<(bool IsValid, List<string> Errors)> UpdateUserValidator(UserUpdateDTO dto);

        Task<IEnumerable<UserModel>> GetUsersAsync();
        Task<string> UpdateUserRoleAsync(int userId, string newRole);
        Task<string> NotifyUserAsync(UserNotifyUpdateDTO dto);

        Task<(bool IsValid, List<string> Errors)> ValidateUserNotifyAsync(UserNotifyUpdateDTO dto);


    }
}
