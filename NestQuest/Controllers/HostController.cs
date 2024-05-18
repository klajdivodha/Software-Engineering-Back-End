using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using NestQuest.Data.DTO;
using NestQuest.Data.DTO.HostDTO;
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
        public async Task<IActionResult> AddProperty([FromBody] AddPropertyDto obj)
        {
            try
            {
                var result = await _hostServices.AddProperty(obj);
                if (result <= 0) { return StatusCode(500, "Internal Server Error"); }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet("ListProperties/{hostId}")]
        public async Task<IActionResult> ListHostProperties(string hostId)
        {
            try
            {
                if (!int.TryParse(hostId, out int ID))
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

        [HttpGet("GetProperty/{propertyId}")]
        public async Task<IActionResult> GetProperty(string propertyId)
        {
            try
            {
                if (!int.TryParse(propertyId, out int ID))
                {
                    return BadRequest("Invalid ID format. ID must be an integer.");
                }
                var result = await _hostServices.PropertyInfo(ID);
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPatch("ChangeAvailability")]
        public async Task<IActionResult> SetAvailability(SetAvailabilityDto dto)
        {
            try
            {
                var result = await _hostServices.SetPropertyAvailability(dto);
                if (result == -1) return NotFound();
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPatch("ChangeNumberOfBeds")]
        public async Task<IActionResult> ChangeNumberOfBeds(string propertyId, BedDto dto)
        {
            try
            {
                if (!int.TryParse(propertyId, out int ID))
                {
                    return BadRequest("Invalid ID format. ID must be an integer.");
                }
                var result = await _hostServices.ChangeNumberOfBeds(ID, dto);

                if(result == -1) return NotFound();
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost("AddTypeOfBed/{propertyId}")]
        public async Task<IActionResult> AddTypeOfBed(string propertyId, BedDto dto)
        {
            try
            {
                if (!int.TryParse(propertyId, out int ID))
                {
                    return BadRequest("Invalid ID format. ID must be an integer.");
                }
                var result = await _hostServices.AddTypeOfBed(ID, dto);

                if (result == -1) return Conflict();
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost("AddUtility/{propertyId}/{utility}")]
        public async Task<IActionResult> AddUtility(string propertyId, string utility)
        {
            try
            {
                if (!int.TryParse(propertyId, out int ID))
                {
                    return BadRequest("Invalid ID format. ID must be an integer.");
                }
                var result = await _hostServices.AddUtility(ID, utility);

                if (result == -1) return Conflict();
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete("DeleteUtility/{propertyId}/{utility}")]
        public async Task<IActionResult> DeleteUtility(string propertyId, string utility)
        {
            try
            {
                if (!int.TryParse(propertyId, out int ID))
                {
                    return BadRequest("Invalid ID format. ID must be an integer.");
                }
                var result = await _hostServices.DeleteUtility(ID, utility);

                if (result == -1) return Conflict();
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("ListReviews/{propertyId}")]
        public async Task<IActionResult> GetReviews(string propertyId)
        {
            try
            {
                if (!int.TryParse(propertyId, out int ID))
                {
                    return BadRequest("Invalid ID format. ID must be an integer.");
                }
                var result = await _hostServices.GetPropertyReviews(ID);
                return Ok(result);
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }

        /*[HttpPatch("ConfirmBooking")]
        public async Task<IActionResult> ConfirmBooking(BookingDto dto)
        {
            try
            {
                var result = await _hostServices.ConfirmBooking(dto);
                if (result == false) return NotFound();
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }*/

        [HttpPatch("RejectBooking")]
        public async Task<IActionResult> RejectBooking(BookingDto dto)
        {
            try
            {
                var result = await _hostServices.RejectBooking(dto);
                if (result == false) return NotFound();
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("GetBookings/{propertyId}")]
        public async Task<IActionResult> GetBookings(string propertyId)
        {
            try
            {
                if (!int.TryParse(propertyId, out int ID))
                {
                    return BadRequest("Invalid ID format. ID must be an integer.");
                }
                var result = await _hostServices.ViewBookings(ID);
                if(result == null) return NotFound();
                return Ok(result);
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }

        [HttpPost("AddReporting")]
        public async Task<IActionResult> AddReporting([FromForm] AddReportingsDto dto)
        {
            try
            {
                var result = await _hostServices.ReportGuest(dto);
                if (result == 0) { return StatusCode(500, "Internal Server Error"); }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPatch("AddGuestRating/{id}/{rating}")]
        public async Task<IActionResult> AddHostRating(string id, string rating)
        {
            try
            {
                var result = await _hostServices.RateGuest(int.Parse(id), int.Parse(rating));
                if (result == null) { return StatusCode(500, "Internal Server Error"); }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpGet("GetRevenue")]
        public async Task<IActionResult> GetRevenue()
        {
            try
            {
                return Ok(await _hostServices.GetRevenue());
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpGet("GetGuest")]
        public async Task<IActionResult> GetGuest(BookingDto dto)
        {
            try
            {
                var result = await _hostServices.GetGuestDetailsByBooking(dto);
                if (result == null) { return NotFound(); };
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

    }
}
