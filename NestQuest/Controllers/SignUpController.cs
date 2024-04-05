using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NestQuest.Data.DTO;
using NestQuest.Services;

namespace NestQuest.Controllers
{
    [Route("api/signup")]
    [ApiController]
    public class SignUpController : ControllerBase
    {
        private readonly ISignUpServices _signUpService;

        public SignUpController(ISignUpServices signUpService)
        {
            _signUpService = signUpService;
        }
        [HttpPost("Guest")]
        public async Task<ActionResult> SignUpGuest([FromBody] SignUpGuestDto userDto)
        {
            try
            {
                var user = await _signUpService.SignUpGuest(userDto);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return NotFound();
            }

        }

        [HttpPost("Host")]
        public async Task<ActionResult> SignUpHost([FromBody] SignUpHostDto Dto)
        {
            try
            {
                var user = await _signUpService.SignUpHost(Dto);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return NotFound();
            }

        }
    }
}
