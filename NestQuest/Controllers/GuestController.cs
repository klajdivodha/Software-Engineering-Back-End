﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NestQuest.Data.DTO;
using NestQuest.Data.Models;
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

        [HttpGet("GetProperty/{id}")]
        public async Task<IActionResult> PropertyInfo(string id)
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

        [HttpPost("CheckAvailability")]
        public async Task<IActionResult> CheckAvailability([FromBody] CheckAvailabilityDto dto)
        {
            try
            {
                var rezult = await _guestServices.CheckAvailability(dto);
                if (rezult) { return Ok(); }
                return Conflict();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }
        [HttpPost("AddBooking")]
        public async Task<IActionResult> AddBookings([FromBody] Bookings obj)
        {
            try
            {
                obj.Status = "upcoming";
                var result= await _guestServices.AddBooking(obj);
                if(result == 0) { return StatusCode(500, "Internal Server Error"); }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPatch("CancelBooking")]
        public async Task<IActionResult> CancelBooking(BookingDto dto)
        {
            try
            {
                var result=await _guestServices.CancelBooking(dto);
                if (result == -1) { return NotFound(); }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet("GetBookings/{id}")]
        public async Task<IActionResult> GetBookings(string id)
        {
            try
            {
                return Ok(await _guestServices.GetBookings(int.Parse(id)));
            }
            catch(Exception ex) 
            {
                return BadRequest();
            }
        }

        [HttpPost("AddReview")]
        public async Task<IActionResult> AddReview([FromBody] AddReviewDto dto)
        {
            try
            {
                var result = await _guestServices.AddReview(dto);
                if (result == 0) {return StatusCode(500, "Internal Server Error"); }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost("AddReporting")]
        public async Task<IActionResult> AddReporting([FromForm] AddReportingsDto dto)
        {
            try
            {
                var result= await _guestServices.AddReporting(dto);
                if (result == 0) { return StatusCode(500, "Internal Server Error"); }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPatch("AddPropertyRatings")]
        public async Task<IActionResult> AddRatings([FromBody] AddRatingsDto dto)
        {
            try
            {
                var rezult= await _guestServices.AddRatings(dto);
                if (rezult == null) { return StatusCode(500, "Internal Server Error"); }
                return Ok(rezult);
            }
            catch (Exception ex)
            { 
                return BadRequest(); 
            }
        }

        [HttpPatch("AddHostRating/{id}/{rating}")]
        public async Task<IActionResult> AddHostRating(string id, string rating)
        {
            try
            {
                var rezult = await _guestServices.RateHost(int.Parse(id),int.Parse(rating));
                if (rezult == null) { return StatusCode(500, "Internal Server Error"); }
                return Ok(rezult);
            }
            catch (Exception ex)
            {
                return BadRequest();    
            }
        }

        [HttpGet("GetHost/{id}")]
        public async Task<IActionResult> GetHost(string id)
        {
            try
            {
                int ID;
                int.TryParse(id, out ID);
                var result = await _guestServices.GetHost(ID);
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
