using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NestQuest.Data.DTO;
using NestQuest.Services;

namespace NestQuest.Controllers
{
    [Route("api/LogIn")]
    [ApiController]
    public class LogInController : ControllerBase
    {
        private readonly ILogInService _logInService;

        public LogInController(ILogInService logInService)
        {
            _logInService = logInService;
        }
        [HttpPost("LogIn")]
        public async Task<ActionResult> LogIn([FromBody] LoginCredencialsDto dto)
        {
            try
            {
                var result = await _logInService.LogIn(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound();
            }

        }

        [HttpPost("Check_2FA")]
        public async Task<ActionResult> Check_2FA([FromBody] FaCredencialsDto dto)
        {
            try
            {
                var result = await _logInService.Confirm_Fa(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound();
            }

        }
        [HttpGet("{email}")]
        public async Task<ActionResult> Resend_2FA(string email)
        {
            try
            {
                var result = _logInService.Resend_Fa(email);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound();
            }

        }
    }
}
