using Microsoft.EntityFrameworkCore;
using NestQuest.Data;
using NestQuest.Data.DTO.AdminDto;
using NestQuest.Data.Models;
using System.Net.Mail;
using System.Net;
using System.Linq;

namespace NestQuest.Services
{
    public interface IAdminServices
    {
        public Task<object> GetAdmin(int id);
        public Task<object[]> GetNotApprovedHosts();
        public Task<int> ApproveHost(int id);
        public Task<int> DontApproveHost(int id);
        public Task<object[]> GetGuestReportings();
        public Task<object[]> GetHostReportings();
        public Task<int> DontApproveReporting(ReportingsDto dto);
        public Task<int> ApproveGuestReporting(ApproveReportingDto dto);
        public Task<int> ApproveHostReporting(AprvHostReportingDto dto);
        public Task<object[]> GetRevenue(); 
    }
    public class AdminServices : IAdminServices
    {
        private readonly DBContext _context;

        public AdminServices(DBContext context)
        {
            _context = context;
        }

        public async Task<int> ApproveGuestReporting(ApproveReportingDto dto)
        {
            try
            {
                var rezult = await _context.Reportings
                    .Where(r => r.Property_Id == dto.Property_id)
                    .ToArrayAsync();
                var rez=rezult.Where(r=>r.Start_Date==dto.StarDate).FirstOrDefault();
                var count = rezult.Where(r => r.Status == "approved" && r.Reporting_User_Type == "guest").Count();
                if ( rez== null ) { return -1; }
                rez.Status = "approved";
                rez.Fine = dto.Fine;
                var NR = await _context.SaveChangesAsync();
                await SideWorkApproveGuestReporting(rez,count);
                return NR;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<int> ApproveHostReporting(AprvHostReportingDto dto)
        {
            try
            {
                var rezult = await _context.Reportings
                    .Where(r => r.Guest_Id==dto.GuestId)
                    .ToArrayAsync();
                var rez = rezult.Where(r => r.Start_Date == dto.StarDate && r.Property_Id==dto.Property_id).FirstOrDefault();
                var count = rezult.Where(r => r.Status == "approved" && r.Reporting_User_Type == "host").Count();
                if (rez == null) { return -1; }
                rez.Status = "approved";
                rez.Fine = dto.Fine;
                var NR = await _context.SaveChangesAsync();
                await SideWorkApproveHostReporting(rez, count);
                return NR;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> SendEmail(string toEmailAddress, string content, string Subject)
        {
            var fromAddress = new MailAddress("nestquest2@gmail.com", "Nest Quest");
            var toAddress = new MailAddress(toEmailAddress);
            const string fromPassword = "rtbt zmpo lngl uajx";
            string subject = Subject;
            string body = content;

            try
            {
                using (var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                })
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    await smtp.SendMailAsync(message);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<int> SideWorkApproveHostReporting(Reportings obj, int count)
        {
            try
            {
                var guest=await _context.Users.Where(u=> u.User_Id==obj.Guest_Id).Select(u => new { u.Guest, u.Name, u.Email }).FirstOrDefaultAsync();
                await SendEmail(guest.Email, $"Hello {guest.Name}! We would like to inform you that you have been fined {obj.Fine} €. Because of the following report: {obj.Description}", "Importat information!");
                if (count >= 2)
                {
                    guest.Guest.banned = true;
                    await _context.SaveChangesAsync();
                    await SendEmail(guest.Email, $"Hello {guest.Name}! We would like to inform you that you have been banned from our app.", "Importat information!");
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<int> SideWorkApproveGuestReporting(Reportings obj, int count)
        {
            try
            {
                var hostId = await _context.Properties.Where(p => p.Property_ID == obj.Property_Id).FirstOrDefaultAsync();
                var host = await _context.Users
                       .Where(u => u.User_Id == hostId.Owner_ID)
                       .Select(u => new { u.Host, u.Name, u.Email })
                       .FirstOrDefaultAsync();
                await SendEmail(host.Email, $"Hello {host.Name}! We would like to inform you that you have been fined {obj.Fine} €. Because of the following report: {obj.Description}", "Importat information!");
                if (count >= 2)
                {
                    hostId.Availability = false;
                    host.Host.banned = true;
                    await _context.SaveChangesAsync();
                    await SendEmail(host.Email, $"Hello {host.Name}! We would like to inform you that you have been banned from our app.","Importat information!");
                }
                return 0;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<int> ApproveHost(int id)
        {
            try
            {
                var rezult=await _context.Host
                        .Where(h=> h.Host_Id == id)
                        .FirstOrDefaultAsync();
                if(rezult == null) { return -1; }
                rezult.aproved=true;

                return await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<int> DontApproveReporting(ReportingsDto dto)
        {
            try
            {
                var rezult= await _context.Reportings
                    .Where(r=> r.Property_Id == dto.Property_id && r.Start_Date == dto.StarDate)
                    .FirstOrDefaultAsync();
                if(rezult == null) { return -1; }
                rezult.Status = "notapproved";
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<int> DontApproveHost(int id)
        {
            try
            {
                var rezult = await _context.Users
                        .Where(u => u.User_Id == id && u.UserType=="host")
                        .FirstOrDefaultAsync();
                if (rezult == null) { return -1; }
                _context.Remove(rezult);
                var rez= await _context.Host
                        .Where(h=>h.Host_Id==id)
                        .FirstOrDefaultAsync();
                _context.Remove(rez);

                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<object> GetAdmin(int id)
        {
                
            try
            {
                var rezult = await _context.Users
                        .Where(u => u.User_Id == id)
                        .FirstOrDefaultAsync();
                if (rezult == null) { return null; }
                return rezult;
            }
            catch (Exception ex)
            {
                throw;
            }
                
        }

        public async Task<object[]> GetGuestReportings()
        {
            try
            {
                var rezult= await _context.Reportings
                        .Where(r=>r.Reporting_User_Type=="guest" && r.Status=="pending")
                        .ToArrayAsync();
                if (rezult == null) { return []; }
                return rezult;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<object[]> GetHostReportings()
        {
            try
            {
                var rezult = await _context.Reportings
                        .Where(r => r.Reporting_User_Type == "host" && r.Status == "pending")
                        .ToArrayAsync();
                if (rezult == null) { return []; }
                return rezult;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<object[]> GetNotApprovedHosts()
        {
            try
            {
                var rezult= await _context.Users
                        .Where(u=>u.UserType=="host" && u.Host.aproved==false)
                        .Select(u => new
                        {
                            u.User_Id,
                            u.Name,
                            u.Surname,
                            u.Email,
                            u.Phone,
                            u.Birthday,
                            u.Nationality,
                            u.Host
                        })
                        .ToArrayAsync();
                if(rezult == null) { return[] ; }
                return rezult;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<object[]> GetRevenue()
        {
            try
            {
                var result = await _context.Bookings
                    .Where(b=>b.Status=="done")
                    .Select(b => new
                {
                    Date = b.End_Date.ToString("yyyy-mm-dd"),
                    Amount=b.Amount*0.1
                }).ToArrayAsync();
                if(result == null) { return []; }
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
