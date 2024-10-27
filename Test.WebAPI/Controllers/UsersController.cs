using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Test.Application.Abstraction;
using Test.Application.DTOs.User;
using Test.Application.Results;
using Test.Domain.Entities;

namespace Test.WebAPI.Controllers
{
    [EnableCors]
    public class UsersController : ControllerBase
    {

        private readonly IUserService _service;


        public UsersController(IUserService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<IEnumerable<UserModel>>>> GetAllUsers()
        {
            ServiceResponse<IEnumerable<UserModel>> response = new();

            response.Payload = await _service.GetUsersAsync();

            return Ok(response);

        }
     
        [HttpPost]
        public async Task<ActionResult<ServiceResponse<string>>> CreateUser([FromBody] UserCreationDTO dto)
        {
            ServiceResponse<string> response = new();


            response.Payload = await _service.CreateUserAsync(dto.Name,dto.Email,dto.Password,dto.Role);

            return Ok(response);

        }

        [HttpPut]
        public async Task<ActionResult<ServiceResponse<string>>> UpdateUserRole([FromForm] UserUpdateDTO dto)
        {
            ServiceResponse<string> response = new();

            response.Payload = await _service.UpdateUserRoleAsync(dto.UserId,dto.RoleId);

            return Ok(response);

        }
    }
}