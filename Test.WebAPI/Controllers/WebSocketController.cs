using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Test.Application.Abstraction;
using Test.Application.DTOs.User;
using Test.Application.Results;

namespace Test.WebAPI.Controllers
{
    [EnableCors]
    public class WebSocketController : ControllerBase
    {
        private readonly IUserService _service;

        public WebSocketController(IUserService service) {

            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<string>>> NotifyUserUpdate([FromBody] UserNotifyUpdateDTO dto)
        {
            ServiceResponse<string> response = new();

            var (isValid, Errors) = await _service.ValidateUserNotifyAsync(dto);

            response.Success = isValid;

            if (!isValid)
            {
                response.Errors = Errors;
                return Ok(response);
            }

            response.Payload = await _service.NotifyUserAsync(dto);

            return Ok(response);

        }
    }
}
