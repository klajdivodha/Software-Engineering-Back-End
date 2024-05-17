using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NestQuest.Data;
using NestQuest.Data.DTO;
using NestQuest.Data.Models;
using NestQuest.Data.DTO.HostDTO;
using System.Net.Mail;
using System.Net;
using System.Diagnostics.Metrics;

namespace NestQuest.Services
{
    public interface IHostServices
    {
        public Task<object[]> ListHostProperties(int hostId);
        public Task<object> PropertyInfo(int propertyId);

        public Task<int> AddProperty(AddPropertyDto obj);
        

        /*public Task<object[]> ViewBookings(int propertyId);
        public Task<ActionResult> BookingDetails(int bookingId);

        public Task<bool> ConfirmBooking(int bookingId);
        public Task<bool> RejectBooking(int bookingId);

        public Task<object[]> GetPropertyReviews(int propertyId);
        public Task<int> RespondToReview(int reviewId, string response);

        public Task<object[]> GetReports(int propertyId);
        public Task<int> ResolveReport(int reportId);*/

        public Task<int> ChangeEmail(int hostId, string newEmail);
        public Task<int> ChangePassword(ChangePasswordDto dto);

        //public Task<object[]> GetGuestDetailsByBooking(int bookingId);

        public Task<int> SetPropertyAvailability(SetAvailabilityDto dto);

    }
    public class HostServices : IHostServices
    {
        private readonly DBContext _context;

        public HostServices(DBContext context)
        {
            _context = context;
        }

        public static async Task<bool> SendEmail(string toEmailAddress, string content, string Subject)
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

        public async Task<object[]> ListHostProperties(int hostId)
        {
            try
            {
                var properties = await _context.Properties
                .Where(p => p.Owner_ID == hostId)
                .ToArrayAsync();

                if (properties.Any())
                    return properties;
                return [];
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        public async Task<object> PropertyInfo(int propertyId)
        {
            try
            {
                var property = await _context.Properties.Where(p => p.Property_ID == propertyId)
                .FirstOrDefaultAsync();
                if (property == null)
                    return null;
                return property;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<int> AddProperty(AddPropertyDto propertyDto)
        {
            try
            {
                var property = new Properties
                {
                    Owner_ID = propertyDto.Owner_ID,
                    Availability = true,
                    Name = propertyDto.Name,
                    Description = propertyDto.Description,
                    Type = propertyDto.Type,
                    Address = propertyDto.Address,
                    City = propertyDto.City,
                    Country = propertyDto.Country,
                    Daily_Price = propertyDto.Daily_Price,
                    Latitude = propertyDto.Latitude,
                    Longitude = propertyDto.Longitude,
                    Preium_Fee_Start = null,
                    Nr_Rooms = propertyDto.Nr_Rooms,
                    Max_Nr_Of_Guests = propertyDto.Max_Nr_Of_Guests,
                    Pets = propertyDto.Pets,
                    Nr_Of_Baths = propertyDto.Nr_Of_Baths,
                    Nr_Of_Bookings = 0,
                    Checkin_Time = propertyDto.Checkin_Time,
                    Checkout_Time = propertyDto.Checkout_Time,
                    Parties = propertyDto.Parties,
                    Smoking = propertyDto.Smoking,
                    Nr_Of_Parking_Spots = propertyDto.Nr_Of_Parking_Spots,
                    Cleanliness_Rating = 5,
                    Accuracy_Rating = 5,
                    Checkin_Rating = 5,
                    Communication_Rating = 5,
                    Location_Rating = 5,
                    Price_Rating = 5,
                    Overall_Rating = 5,
                    Nr_Of_Ratings = 1,

                    Beds = new List<Beds>(),
                    Utilities = new List<Utilities>()
                };

                foreach (var bedDto in propertyDto.Beds)
                {
                    property.Beds.Add(new Beds
                    {
                        Type = bedDto.Type,
                        Number = bedDto.Number,
                    });

                }

                foreach (var utilityDto in propertyDto.Utilities)
                {
                    property.Utilities.Add(new Utilities
                    {
                        Utilitie = utilityDto.Utilitie,
                    });
                }

                await _context.Properties.AddAsync(property);
                await _context.SaveChangesAsync();
                return property.Property_ID;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Task<int> UpdateProperty(int propertyId, UpdatePropertyDto dto)
        {
            return;
        }

        /*public Task<object[]> ViewBookings(int propertyId)
        {
            return;
        }*/
        /*public Task<ActionResult> BookingDetails(int bookingId)
        {
            return;
        }*/

        /*public Task<bool> ConfirmBooking(int bookingId)
        {
            return;
        }*/
        /*public Task<bool> RejectBooking(int bookingId)
        {
            return;
        }*/

        /*public Task<object[]> GetPropertyReviews(int propertyId)
        {
            return;
        }*/
        /*public Task<int> RespondToReview(int reviewId, string response)
        {
            return;
        }*/

        /*public Task<object[]> GetReports(int propertyId)
        {
            return;
        }*/
        /*public Task<int> ResolveReport(int reportId)
        {
            return;
        }*/

        public async Task<int> ChangeEmail(int hostId, string email)
        {
            try
            {
                var result = await _context.Users
                            .Where(u => u.User_Id == hostId)
                            .FirstOrDefaultAsync();

                if (result == null) { return -1; }
                var condition = await _context.Users.Where(e => e.Email == email).FirstOrDefaultAsync();
                if (condition != null)
                {
                    return -2;
                }
                result.Email = email;
                var nr = await _context.SaveChangesAsync();

                SendEmail(email, $"{result.Name} your email address has been changed this is your new email address that will be used in our app.",
                    "Email Change");

                return nr;

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<int> ChangePassword(ChangePasswordDto dto)
        {
            try
            {
                var result = await _context.Users
                        .Where(u => u.User_Id == dto.Id)
                        .FirstOrDefaultAsync();
                if (result == null) { return -1; }
                if (GuestServices.VerifyPassword(result.Password, dto.Password))
                {
                    result.Password = GuestServices.HashPassword(dto.NewPassword);

                    var nr = _context.SaveChanges();
                    return nr;
                }
                else
                {
                    return -2;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /*public Task<object[]> GetGuestDetailsByBooking(int bookingId)
        {
            return;
        }*/

        public async Task<int> SetPropertyAvailability(SetAvailabilityDto dto)
        {
            var result = await _context.Properties.Where(p => p.Property_ID == dto.Property_ID)
                                                  .FirstOrDefaultAsync();
            if (result == null) { return -1; }

            result.Availability = dto.Availability;
            var nr = _context.SaveChanges();

            return nr;
        }
    }
}
