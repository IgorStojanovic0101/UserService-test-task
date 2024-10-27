using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Application.Abstraction;
using Test.Application.DTOs.User;
using Test.Application.Services;
using Test.Application.Utilities;
using Test.Application.Validators;

namespace Test.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddScoped<IValidator<UserUpdateDTO>, UpdateUserRoleValidator>();
            services.AddScoped<IValidator<UserCreationDTO>, UserCreationDTOValidator>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IErrorService, ErrorService>();



            return services;
        }
    }
}
