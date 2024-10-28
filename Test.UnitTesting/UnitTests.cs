using FakeItEasy;
using FluentValidation;
using Microsoft.Extensions.Caching.Memory;
using MongoDB.Driver.Core.Connections;
using Org.BouncyCastle.Tls;
using Test.Application.Abstraction;
using Test.Application.DTOs.User;
using Test.Application.Services;
using Test.Domain.Entities;
using Test.Domain.Enums;
using Test.Domain.Repositories;

namespace Test.UnitTesting
{
    public class UnitTests
    {
        private readonly IUserRepository _userRepository;
        private readonly IValidator<UserCreationDTO> _userCreationValidator;
        private readonly IValidator<UserUpdateDTO> _updateUserRoleValidator;
        private readonly IMemoryCache _cache;
        private readonly IHubNotifier _hubNotifierService;
        private readonly UserService _userService;

        public UnitTests()
        {
            _userRepository = A.Fake<IUserRepository>();
            _userCreationValidator = A.Fake<IValidator<UserCreationDTO>>();
            _updateUserRoleValidator = A.Fake<IValidator<UserUpdateDTO>>();
            //_cache = A.Fake<IMemoryCache>();
            _cache = new MemoryCache(new MemoryCacheOptions()); 
            _hubNotifierService = A.Fake<IHubNotifier>();


            _userService = new UserService(_userRepository, _userCreationValidator, _updateUserRoleValidator, _cache, _hubNotifierService);
        }

        [Fact]
        public async Task NotifyUserAsync_ShouldReturnSuccessMessage_WhenUserExistsAndIsCached()
        {
            // Arrange
            var dto = new UserNotifyUpdateDTO ("test@gmail.com", "Hello");
            var user = new UserModel { Id = 1, Email = dto.Email };
            _cache.Set("1", "123");
            var connectionId = "";


            A.CallTo(() => _userRepository.GetUsersAsync()).Returns(new List<UserModel> { user });
             _cache.TryGetValue(user.Id.ToString(), out connectionId);

            // Act
            var result = await _userService.NotifyUserAsync(dto);

            // Assert
            Assert.Equal("Notifikacija je poslata", result);
           
        }

        [Fact]
        public async Task NotifyUserAsync_ShouldReturnFailureMessage_WhenUserIsNotInCache()
        {
            // Arrange
            var dto = new UserNotifyUpdateDTO ("test@gmail.com", "Hello");
            var user = new UserModel { Id = 1, Email = dto.Email };

            _cache.Set("1", "");

            string connectionId = "";

            A.CallTo(() => _userRepository.GetUsersAsync()).Returns(new List<UserModel> { user });
            _cache.TryGetValue(user.Id.ToString(), out connectionId);

            // Act
            var result = await _userService.NotifyUserAsync(dto);

            // Assert
            Assert.Equal("Notifikacija je neuspesno poslata", result);
          
        }

        [Fact]
        public async Task CreateUserAsync_ShouldReturnSuccessMessage_WhenUserIsCreated()
        {
            // Arrange
            var name = "Igor Stojanovic";
            var email = "igor.stojanovic@gmail.com";
            var password = "password";
            var role = "user";

            A.CallTo(() => _userRepository.CreateUserAsync(name, email, A<string>._, role)).Returns(ResultEnum.Success);

            // Act
            var result = await _userService.CreateUserAsync(name, email, password, role);

            // Assert
            Assert.Equal("Korisnik je uspesno kreiran.", result);
        }

        [Fact]
        public async Task ValidateUserNotifyAsync_ShouldReturnError_WhenUserNotFound()
        {
            // Arrange
            var dto = new UserNotifyUpdateDTO("empty@gmail.com", "Hello");

            A.CallTo(() => _userRepository.GetUsersAsync()).Returns(new List<UserModel>());

            // Act
            var (isValid, errors) = await _userService.ValidateUserNotifyAsync(dto);

            // Assert
            Assert.False(isValid);
            Assert.Contains("Korisnik sa ovim Email-om ne postoji.", errors);
        }

        [Fact]
        public async Task UpdateUserRoleAsync_ShouldReturnSuccessMessage_WhenRoleUpdatedSuccessfully()
        {
            // Arrange
            var userId = 1;
            var newRole = "Admin";

            A.CallTo(() => _userRepository.UpdateUserRoleAsync(userId, newRole)).Returns(ResultEnum.Success);

            // Act
            var result = await _userService.UpdateUserRoleAsync(userId, newRole);

            // Assert
            Assert.Equal("Korisnik je uspesno azuriran.", result);
        }
    }
}