using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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

        [HttpGet("GetPropertie/{id}")]
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
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("GetGuest/{id}")]

        public async Task<IActionResult> GetGuest(string id)
        {
            try
            {
                int ID;
                int.TryParse(id, out ID);
                var result = await _guestServices.GetGuest(ID);
                if (result == null) { return NotFound(); };
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPatch("ChangeEmail/{id}/{email}")]
        public async Task<IActionResult> ChangeEmailAsync(string id, string email)
        {
            try
            {
                int ID;
                int.TryParse(id, out ID);
                var rezult = await _guestServices.ChangeEmail(ID, email);
                if (rezult == -1) { return NotFound(); }
                if (rezult == -2) { return Conflict(); }
                if (rezult == 0) { return StatusCode(500, "Internal Server Error"); }
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
            var rezult = -3;
            try
            {
                Console.WriteLine(rezult);
                rezult = await _guestServices.ChangePassword(dto);
                if (rezult == -1) { return NotFound(); }
                if (rezult == -2) { return Conflict(); }
                if (rezult == 0) { return StatusCode(500, "Internal Server Error"); }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost("AddFavorites/{guest_id}/{property_id}")]
        public async Task<IActionResult> AddFavorites(string guest_id, string property_id)
        {
            try
            {
                var result = await _guestServices.AddFavorites(int.Parse(guest_id), int.Parse(property_id));
                if (result == 0) { return StatusCode(500, "Internal Server Error"); }
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete("DeleteFavorite/{guest_id}/{property_id}")]
        public async Task<IActionResult> DeleteFavorite(string guest_id, string property_id)
        {
            try
            {
                var result = await _guestServices.DeleteFavorites(int.Parse(guest_id), int.Parse(property_id));
                if(result == -1) { return NotFound(); }
                if (result == 0) { return StatusCode(500, "Internal Server Error"); }
                return Ok();
            }
            catch(Exception ex) { 
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetFavorites/{id}")]
        public async Task<IActionResult> GetFavorites(string id)
        {
            try
            {
                var result= await _guestServices.GetFavorites(int.Parse(id));
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
