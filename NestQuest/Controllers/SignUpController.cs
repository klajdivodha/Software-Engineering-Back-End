using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NestQuest.Data.DTO;
using NestQuest.Services;

namespace NestQuest.Controllers
{
    [Route("api/Signup")]
    [ApiController]
    public class SignUpController : ControllerBase
    {
        private readonly ISignUpServices _signUpService;

        public SignUpController(ISignUpServices signUpService)
        {
            _signUpService = signUpService;
        }
        [HttpPost("SignUpGuest")]
        public async Task<ActionResult> SignUpGuest([FromBody] SignUpGuestDto userDto)
        {
            try
            {
                var user = await _signUpService.SignUpGuest(userDto);
                if (user == null) { return Conflict(); }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        [HttpPost("SignUpHost")]
        public async Task<ActionResult> SignUpHost([FromBody] SignUpHostDto Dto)
        {
            try
            {
                var user = await _signUpService.SignUpHost(Dto);
                if (user == null) { return Conflict(); }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }
    }
}
