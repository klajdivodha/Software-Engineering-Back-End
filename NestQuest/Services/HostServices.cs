using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NestQuest.Data;
using NestQuest.Data.DTO;
using NestQuest.Data.Models;
using NestQuest.Data.DTO.HostDTO;
using System.Net.Mail;
using System.Net;
using System.Diagnostics.Metrics;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static System.Net.Mime.MediaTypeNames;

namespace NestQuest.Services
{
    public interface IHostServices
    {
        public Task<int> ChangeEmail(int hostId, string newEmail);
        public Task<int> ChangePassword(ChangePasswordDto dto);
        public Task<int> AddProperty(AddPropertyDto obj);
        public Task<object> PropertyInfo(int propertyId);
        public Task<object[]> ListHostProperties(int hostId);
        public Task<int> SetPropertyAvailability(SetAvailabilityDto dto);
        public Task<int> ChangeNumberOfBeds(int propertyId, BedDto newBeds);
        public Task<int> AddTypeOfBed(int propertyId, BedDto dto);
        public Task<int> AddUtility(int propertyId, string utility);
        public Task<int> DeleteUtility(int propertyId, string utility);
        public Task<object[]> GetPropertyReviews(int propertyId);
        //public Task<bool> ConfirmBooking(BookingDto dto);
        public Task<bool> RejectBooking(BookingDto dto);
        public Task<object[]> ViewBookings(int propertyId);
        public Task<int> ReportGuest(AddReportingsDto dto);
        public Task<object> RateGuest(int hostId, double rating);
        public Task<object[]> GetRevenue();
        //add photos when adding a property and fix list all properties function

        public Task<object> GetGuestDetailsByBooking(DateTime startDate, DateTime bookingTime, int propertyId);



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

                await SendEmail(email, $"{result.Name} your email address has been changed this is your new email address that will be used in our app.",
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

                string photosDirectoryPath = @"C:\Users\user\Desktop\photos\properties";

                if (!Directory.Exists(photosDirectoryPath))
                {
                    Directory.CreateDirectory(photosDirectoryPath);
                }

                int i = 0;

                foreach (var imageDto in propertyDto.Images)
                {
                    string fileName = $"{property.Property_ID}{i}.jpg";
                    string filePath = Path.Combine(photosDirectoryPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageDto.CopyToAsync(stream);
                    }
                    await _context.SaveChangesAsync();
                    i++;
                }

                return property.Property_ID;
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
                                                        .Include(p => p.Utilities)
                                                        .Include(p => p.Beds)
                                                        .Include(p => p.Reviews)
                                                        .FirstOrDefaultAsync();
                if (property == null)
                    return null;

                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve
                };

                var serializedResult = JsonSerializer.Serialize(property, options);

                return new JsonResult(serializedResult);
            }
            catch (Exception)
            {

                throw;
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


        public async Task<int> SetPropertyAvailability(SetAvailabilityDto dto)
        {
            var result = await _context.Properties.Where(p => p.Property_ID == dto.Property_ID)
                                                  .FirstOrDefaultAsync();
            if (result == null) { return -1; }

            result.Availability = dto.Availability;
            var nr = _context.SaveChanges();

            return nr;
        }

        public async Task<int> ChangeNumberOfBeds(int propertyId, BedDto newBeds)
        {
            try
            {
                var beds = await _context.Beds.Where(p => p.Property_ID == propertyId && p.Type == newBeds.Type)
                                                  .FirstOrDefaultAsync();
                if (beds == null) { return -1; }

                beds.Number = newBeds.Number;
                var nr = _context.SaveChanges();
                return nr;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<int> AddTypeOfBed(int propertyId, BedDto dto)
        {
            try
            {
                if (await CheckBedTypeExists(propertyId, dto.Type))
                { return -1; }

                Beds newBed = new Beds();
                newBed.Number = dto.Number;
                newBed.Type = dto.Type;
                newBed.Property_ID = propertyId;

                await _context.Beds.AddAsync(newBed);
                await _context.SaveChangesAsync();
                return newBed.Property_ID;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<bool> CheckBedTypeExists(int propertyId, string bedType)
        {
            return await _context.Beds.AnyAsync(b => b.Property_ID == propertyId && b.Type == bedType);
        }

        public async Task<int> AddUtility(int propertyId, string utility)
        {
            try
            {
                if (await CheckUtilityExists(propertyId, utility))
                { return -1; }

                Utilities newUtility = new Utilities();
                newUtility.Utilitie = utility; 
                newUtility.Property_ID = propertyId;

                await _context.Utilities.AddAsync(newUtility);
                await _context.SaveChangesAsync();
                return newUtility.Property_ID;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<int> DeleteUtility(int propertyId, string utility)
        {
            try
            {
                var utilityObject = await _context.Utilities
                                                  .FirstOrDefaultAsync(u => u.Property_ID == propertyId && u.Utilitie == utility);

                if (utilityObject == null)
                {
                    return -1;
                }

                _context.Utilities.Remove(utilityObject);
                await _context.SaveChangesAsync();

                return 1;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> CheckUtilityExists(int propertyId, string utility)
        {
            return await _context.Utilities.AnyAsync(u => u.Property_ID == propertyId && u.Utilitie == utility);
        }

        public async Task<object[]> GetPropertyReviews(int propertyId)
        {
            try
            {
                var reviews = await _context.Reviews.Where(p => p.Property_ID == propertyId)
                .ToArrayAsync();
                if (reviews.Any())
                    return reviews;

                return [];
            }
            catch (Exception)
            {

                throw;
            }
        }

        /*public async Task<bool> ConfirmBooking(BookingDto dto)
        {
            try
            {
                var booking = await _context.Bookings
                                            .FirstOrDefaultAsync(u => u.BookingTime == dto.BookingTime
                                                                 && u.Property_Id == dto.Property_Id
                                                                 && u.Start_Date == dto.StartDate);
                if (booking == null) return false;
                booking.Status = "accepted";
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }*/

        public async Task<bool> RejectBooking(BookingDto dto)
        {
            try
            {
                var booking = await _context.Bookings
                                            .FirstOrDefaultAsync(u => u.BookingTime == dto.BookingTime
                                                                 && u.Property_Id == dto.Property_Id
                                                                 && u.Start_Date == dto.StartDate);
                if (booking == null || booking.Status != "upcoming") return false;
                booking.Status = "canceled";
                await _context.SaveChangesAsync();

                var guest = await _context.Users.Where(g => g.User_Id == booking.Guest_Id)
                                                .FirstOrDefaultAsync();

                if(guest == null) return false;
                await SendEmail(guest.Email, $" Hello {guest.Name}, your booking has been rejected by the host of the property.", "Rejected Booking");


                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<object[]> ViewBookings(int propertyId)
        {
            var booking = await _context.Bookings
                                        .Where(u => u.Property_Id == propertyId).ToArrayAsync();

            return booking;
        }

        public async Task<int> ReportGuest(AddReportingsDto dto)
        {
            try
            {
                var report = new Reportings()
                {
                    Property_Id = dto.Property_Id,
                    Start_Date = dto.Start_Date,
                    BookingTime = dto.BookingTime,
                    Guest_Id = dto.Guest_Id,
                    Reporting_User_Type = "host",
                    Status = "pending",
                    Fine = null,
                    Description = dto.Description
                };

                await _context.Reportings.AddAsync(report);

                string photosDirectoryPath = @"C:\Users\user\Desktop\photos\reportings";

                string fileName = $"{dto.Property_Id}{dto.Start_Date.ToString("yyyy-MM-dd")}.jpg";

                if (!Directory.Exists(photosDirectoryPath))
                {
                    Directory.CreateDirectory(photosDirectoryPath);
                }

                string filePath = Path.Combine(photosDirectoryPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.photo.CopyToAsync(stream);
                }
                return await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<object> RateGuest(int guestId, double rating)
        {
            try
            {
                var result = await _context.Guest.Where(h => h.Guest_Id == guestId).FirstOrDefaultAsync();

                if (result == null) { return null; }
                result.rating += rating;
                result.Nr_Of_Ratings += 1;
                var nr = await _context.SaveChangesAsync();
                if (nr == 0)
                {
                    return null;
                }
                return new { result.rating, result.Nr_Of_Ratings };
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
                    .Where(b => b.Status == "done")
                    .Select(b => new
                    {
                        Date = b.End_Date.ToString("yyyy-mm-dd"),
                        Amount = b.Amount * 0.9
                    }).ToArrayAsync();
                if (result == null) { return []; }
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<object> GetGuestDetailsByBooking(DateTime startDate, DateTime bookingTime, int propertyId)
        {
            try
            {
                var booking = await _context.Bookings.Where(b => b.Property_Id == propertyId
                                                        && b.Start_Date == startDate
                                                        && b.BookingTime == bookingTime)
                                                        .FirstOrDefaultAsync();
                if (booking == null) { return null; }
                int GuestId = booking.Guest_Id;

                var guest = await _context.Users.Where(g => g.User_Id == GuestId)
                                          .Select(g => new
                                          {
                                              g.Name,
                                              g.Surname,
                                              g.Email,
                                              g.Phone,
                                              g.Birthday,
                                              g.UserType,
                                              g.Nationality,
                                              g.Guest
                                          }).FirstOrDefaultAsync();
                if(guest == null) { return null; }

                return guest;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
