using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Domain.Repositories;
using Test.Infrastructure.Repositories;
using Test.Infrastructure.UnitOfWork;

namespace Test.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            var connectionStr = configuration.GetConnectionString("DefaultConnection");

            services.AddScoped(provider =>
            {
                var connection = new SqlConnection(connectionStr);
                if (connection.State != ConnectionState.Open)
                {
                     connection.Open();
                }
                return connection;
            });


            services.AddScoped<IDapperRepository,DapperRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
