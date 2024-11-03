using Microsoft.AspNetCore.Mvc;
using TechnicalTest.Core.DTOs;
using TechnicalTest.Core.Interfaces;

namespace TechnicalTest.API.Controllers
{
    [ApiController()]
    [Route("[controller]/API")]
    public class AccountController : ControllerBase
    {
        private readonly IRegistrationService _registrationService;

        public AccountController(IRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserRequestLogin request)
        {
            try
            {
                var login = await _registrationService.LoginUserAsync(request);

                if (!login.IsSuccess)
                {
                    return BadRequest(login.Response);
                }

                return Ok(login.Response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new BaseResponse<UserResponseBasic>()
                    {
                        Message = ex.ToString()
                    });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRequest request)
        {
            try
            {
                var register = await _registrationService.RegisterUserAsync(request);

                if (!register.IsSuccess)
                {
                    return BadRequest(register.Response);
                }

                return Ok(register.Response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new BaseResponse<UserResponse>()
                    {
                        Message = ex.ToString()
                    });
            }
        }
    }
}
