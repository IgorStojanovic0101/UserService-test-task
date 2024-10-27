using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Domain.Repositories;

namespace Test.Infrastructure.Repositories
{
    public class DapperRepository : IDapperRepository
    {
        private SqlConnection _connection;
        public DapperRepository(SqlConnection connection) {
            _connection = connection; 
         
        }
        

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object parameters = null) => await _connection.QueryAsync<T>(sql, parameters);


        public async Task<int> ExecuteAsync(string sql, object parameters = null) => await _connection.ExecuteAsync(sql, parameters);

        
        
    }
}
