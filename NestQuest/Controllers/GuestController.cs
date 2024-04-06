using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NestQuest.Data.DTO;
using NestQuest.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NestQuest.Controllers
{
    [Route("api/Guests")]
    [ApiController]
    public class GuestController : ControllerBase
    {
        private readonly IGuestServices _guestServices;

        public GuestController(IGuestServices guestServices)
        {
            _guestServices = guestServices;
        }

        [HttpPost("AvaibleProperties")]
        public async Task<IActionResult> AvaibleProperties([FromBody] GestAvaiblePropertiesDto dto)
        {
            try
            {
                var result = await _guestServices.GuestAvaibleProperties(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> PropertieInfo(string id)
        {
            try
            {
                int ID;
                int.TryParse(id, out ID);

                var result = await _guestServices.PropertieInfo(ID);
                if (result == null) { return NotFound(); }
                return Ok(result);
            }
            catch {
                return BadRequest();
            }
           
        }
    }
}
