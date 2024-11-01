﻿using FluentValidation;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Test.Application.Abstraction;
using Test.Application.DTOs.User;
using Test.Application.Utilities;
using Test.Domain.Entities;
using Test.Domain.Enums;
using Test.Domain.Repositories;

namespace Test.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IValidator<UserCreationDTO> _userCreationValidator;
        private readonly IValidator<UserUpdateDTO> _updateUserRoleValidator;
        private readonly IMemoryCache _cache;
        private readonly IUserRepository _userRepository;
        private readonly IHubNotifier _hubNotifierService;


        public UserService(IUserRepository userRepository, IValidator<UserCreationDTO> userCreationValidator, IValidator<UserUpdateDTO> updateUserRoleValidator, IMemoryCache cache, IHubNotifier hubNotifierService)
        {
            _userRepository = userRepository;
            _userCreationValidator = userCreationValidator;
            _updateUserRoleValidator = updateUserRoleValidator;
            _cache = cache;
            _hubNotifierService = hubNotifierService;
        }

        public async Task<string> CreateUserAsync(string name, string email, string password, string role)
        {
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            var result =  await _userRepository.CreateUserAsync(name, email, passwordHash, role);

            switch (result)
            {
                case ResultEnum.Success:
                    return "Korisnik je uspesno kreiran.";
                case ResultEnum.NoChange:
                    return "Baza je nepromenjena.";
                case ResultEnum.Error:
                default:
                    return "Doslo je do greske";

            }

        }


        public async Task<(bool IsValid, List<string> Errors)> CreateUserValidator(UserCreationDTO dto)
        {
            var result = await _userCreationValidator.ValidateAsync(dto);
            var errors = result.IsValid ? new List<string>() : result.Errors.Select(x => x.ErrorMessage).ToList();
            return (result.IsValid, errors);
        }



        public async Task<IEnumerable<UserModel>> GetUsersAsync()
        {
          return await _userRepository.GetUsersAsync();
        }

       public async Task<(bool IsValid, List<string> Errors)> ValidateUserNotifyAsync(UserNotifyUpdateDTO dto)
        {
            var errors = new List<string>();

            var user = (await _userRepository.GetUsersAsync()).FirstOrDefault(x => x.Email == dto.Email);

            if (user == null)
            {
                errors.Add("Korisnik sa ovim Email-om ne postoji.");
            }
            else if (!_cache.TryGetValue(user.Id.ToString(), out string connectionId) || string.IsNullOrEmpty(connectionId))
            {
                errors.Add("Korisnik nije zabelezen u cache, konektujete se prvo.");
            }

            return (errors.Count == 0, errors);
        }



        public async Task<string> NotifyUserAsync(UserNotifyUpdateDTO dto)
        {
            var users =  await _userRepository.GetUsersAsync();
            var user = users.FirstOrDefault(x => x.Email == dto.Email);
            if (_cache.TryGetValue(user.Id.ToString(), out string connectionId))
            {
                if (!string.IsNullOrEmpty(connectionId))
                {
                    await _hubNotifierService.NotifyUser(dto.Message, connectionId);
                    return "Notifikacija je poslata";
                }

            }

            return "Notifikacija je neuspesno poslata";

        }

        public async Task<string> UpdateUserRoleAsync(int userId, string newRole)
        {
            var result = await _userRepository.UpdateUserRoleAsync(userId, newRole);

            switch (result)
            {
                case ResultEnum.Success:
                    return "Korisnik je uspesno azuriran.";
                case ResultEnum.NoChange:
                    return "Baza je nepromenjena.";
                case ResultEnum.Error:
                default:
                    return "Doslo je do greske";
            }

        }

        public async Task<(bool IsValid, List<string> Errors)> UpdateUserValidator(UserUpdateDTO dto)
        {
            var result = await _updateUserRoleValidator.ValidateAsync(dto);
            var errors = result.IsValid ? new List<string>() : result.Errors.Select(x => x.ErrorMessage).ToList();
            return (result.IsValid, errors);
        }

       
    }
}
