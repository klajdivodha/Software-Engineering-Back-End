using Microsoft.AspNetCore.Mvc;
using NestQuest.Data.DTO;
using NestQuest.Data.Models;
using NestQuest.Services;

namespace NestQuest.Controllers
{
    [Route("api/Hosts")]
    [ApiController]
    public class HostController : ControllerBase
    {
        private readonly IHostServices _hostServices;

        public HostController(IHostServices hostServices)
        {
            _hostServices = hostServices;
        }

        [HttpPatch("ChangeEmail/{id}/{email}")]
        public async Task<IActionResult> ChangeEmailAsync(string id, string email)
        {
            try
            {
                int ID;
                int.TryParse(id, out ID);
                var result = await _hostServices.ChangeEmail(ID, email);
                if (result == -1) { return NotFound(); }
                if (result == -2) { return Conflict(); }
                if (result == 0) { return StatusCode(500, "Internal Server Error"); }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPatch("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var result = -3;
            try
            {
                Console.WriteLine(result);
                result = await _hostServices.ChangePassword(dto);
                if (result == -1) { return NotFound(); }
                if (result == -2) { return Conflict(); }
                if (result == 0) { return StatusCode(500, "Internal Server Error"); }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost("AddProperty")]
        public async Task<IActionResult> AddProperty([FromBody] Properties obj)
        {
            try
            {
                var result = await _hostServices.AddProperty(obj);
                if (result == 0) { return StatusCode(500, "Internal Server Error"); }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet("ListProperties/{id}")]
        public async Task<IActionResult> ListHostProperties(string id)
        {
            try
            {
                if (!int.TryParse(id, out int ID))
                {
                    return BadRequest("Invalid ID format. ID must be an integer.");
                }
                var result = await _hostServices.ListHostProperties(ID);
                return Ok(result);
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }

    }
}
