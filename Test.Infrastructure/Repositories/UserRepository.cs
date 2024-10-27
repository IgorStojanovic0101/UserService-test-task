using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Domain.Entities;
using Test.Domain.Enums;
using Test.Domain.Repositories;


namespace Test.Infrastructure.UnitOfWork
{

    public class UserRepository : IUserRepository
    {
        private readonly IDapperRepository _dapperRepository;

        public UserRepository(IDapperRepository dapperRepository)
        {
            _dapperRepository = dapperRepository;
        }

        public async Task<ResultEnum> CreateUserAsync(string name, string email, string passwordHash, string role)
        {
            var sql = "INSERT INTO Users (Name, Email, Password, Role) VALUES (@Name, @Email, @Password, @Role); SELECT CAST(SCOPE_IDENTITY() as int);";

           var result = await _dapperRepository.ExecuteAsync(sql, new { Name = name, Email = email, Password = passwordHash, Role = role });

            ResultEnum resultEnum = (ResultEnum) (result);

            return resultEnum;
        }

        public async Task<IEnumerable<UserModel>> GetUsersAsync()
        {
            var sql = "SELECT Id, Name, Email, Password, Role FROM Users";

            return await _dapperRepository.QueryAsync<UserModel>(sql);
        }

        public async Task<ResultEnum> UpdateUserRoleAsync(int userId, string newRole)
        {
            var sql = "UPDATE Users SET Role = @Role WHERE Id = @UserId";

            var result = await _dapperRepository.ExecuteAsync(sql, new { Role = newRole, UserId = userId });
            ResultEnum resultEnum = (ResultEnum)(result);

            return resultEnum;
        }


    }
}
