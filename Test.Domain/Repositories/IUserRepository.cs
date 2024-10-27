using Test.Domain.Entities;
using Test.Domain.Enums;

namespace Test.Domain.Repositories
{
    public interface IUserRepository 
    {
        Task<ResultEnum> CreateUserAsync(string name, string email, string passwordHash, string role);
        Task<IEnumerable<UserModel>> GetUsersAsync();
        Task<ResultEnum> UpdateUserRoleAsync(int userId, string newRole);

    }
}
