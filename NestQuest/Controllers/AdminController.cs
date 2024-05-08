using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NestQuest.Services;

namespace NestQuest.Controllers
{
    [Route("api/Admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminServices _adminServices;

        public AdminController(IAdminServices adminServices)
        {
            _adminServices = adminServices;
        }

        [HttpGet("GetAdmin/{id}")]
        public async Task<IActionResult> GetAdmin(string id)
        {
            try
            {
                var rezult = await _adminServices.GetAdmin(int.Parse(id));
                if (rezult == null) { return NotFound(); }
                return Ok(rezult);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpGet("GetNotApprovedHosts")]
        public async Task<IActionResult> GetNotApprovedHosts()
        {
            try
            {
                var rezult = await _adminServices.GetNotApprovedHosts();
                return Ok(rezult);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPatch("ApproveHost/{id}")]
        public async Task<IActionResult> ApproveHost(string id)
        {
            try
            {
                var result= await _adminServices.ApproveHost(int.Parse(id));
                if (result == -1) { return NotFound();}
                else if (result == 0) { return StatusCode(500, "Internal Server Error"); }
                return Ok();
            }
            catch (Exception ex) 
            {
                return BadRequest();
            }
        }

        [HttpDelete("DontApproveHost/{id}")]
        public async Task<IActionResult> DontApproveHost(string id)
        {
            try
            {
                var rezult= await _adminServices.DontApproveHost(int.Parse(id));
                if(rezult == -1) { return NotFound(); } 
                if (rezult == 0) { return StatusCode(500, "Internal Server Error");}
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpGet("GetGuestReportings")]
        public async Task<IActionResult> GetGuestReportings()
        {
            try
            {
                return Ok(await _adminServices.GetGuestReportings());
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpGet("GetHostReportings")]
        public async Task<IActionResult> GetHostReportings()
        {
            try
            {
                return Ok(await _adminServices.GetHostReportings());
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
