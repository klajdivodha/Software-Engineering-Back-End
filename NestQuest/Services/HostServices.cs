using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NestQuest.Data;
using NestQuest.Data.DTO;
using NestQuest.Data.DTO.HostDTO;

namespace NestQuest.Services
{
    public interface IHostServices
    {
        public Task<object[]> ListHostProperties(int hostId);
        public Task<ActionResult> PropertyInfo(int propertyId);

        public Task<int> AddProperty(AddPropertyDto dto);
        public Task<int> UpdateProperty(int propertyId, UpdatePropertyDto dto);

        public Task<object[]> ViewBookings(int propertyId);
        public Task<ActionResult> BookingDetails(int bookingId);

        public Task<bool> ConfirmBooking(int bookingId);
        public Task<bool> RejectBooking(int bookingId);

        public Task<object[]> GetPropertyReviews(int propertyId);
        public Task<int> RespondToReview(int reviewId, string response);

        public Task<object[]> GetReports(int propertyId);
        public Task<int> ResolveReport(int reportId);

        public Task<int> UpdateHostProfile(int hostId, UpdateHostProfileDto dto);
        public Task<int> ChangeEmail(int hostId, string newEmail);
        public Task<int> ChangePassword(ChangePasswordDto dto);

        public Task<object[]> GetGuestDetailsByBooking(int bookingId);

        public Task<int> SetPropertyAvailability(SetAvailabilityDto dto);
        public Task<int> UpdateBookingAvailability(int propertyId, UpdateAvailabilityDto dto);
    }
    public class HostServices : IHostServices
    {
        private readonly DBContext _context;

        public HostServices(DBContext context)
        {
            _context = context;
        }

        public Task<object[]> ListHostProperties(int hostId)
        {
            return;
        }
        public Task<ActionResult> PropertyInfo(int propertyId)
        {
            return;
        }

        public Task<int> AddProperty(AddPropertyDto dto)
        {
            return;
        }
        public Task<int> UpdateProperty(int propertyId, UpdatePropertyDto dto)
        {
            return;
        }

        public Task<object[]> ViewBookings(int propertyId)
        {
            return;
        }
        public Task<ActionResult> BookingDetails(int bookingId)
        {
            return;
        }

        public Task<bool> ConfirmBooking(int bookingId)
        {
            return;
        }
        public Task<bool> RejectBooking(int bookingId)
        {
            return;
        }

        public Task<object[]> GetPropertyReviews(int propertyId)
        {
            return;
        }
        public Task<int> RespondToReview(int reviewId, string response)
        {
            return;
        }

        public Task<object[]> GetReports(int propertyId)
        {
            return;
        }
        public Task<int> ResolveReport(int reportId)
        {
            return;
        }

        public Task<int> UpdateHostProfile(int hostId, UpdateHostProfileDto dto)
        {
            return;
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

                GuestServices.SendEmail(email, $"{result.Name} your email address has been changed this is your new email address that will be used in our app.",
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
                var rezult = await _context.Users
                        .Where(u => u.User_Id == dto.Id)
                        .FirstOrDefaultAsync();
                if (rezult == null) { return -1; }
                if (GuestServices.VerifyPassword(rezult.Password, dto.Password))
                {
                    rezult.Password = GuestServices.HashPassword(dto.NewPassword);

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

        public Task<object[]> GetGuestDetailsByBooking(int bookingId)
        {
            return;
        }

        public Task<int> SetPropertyAvailability(SetAvailabilityDto dto)
        {
            return;
        }
        public Task<int> UpdateBookingAvailability(int propertyId, UpdateAvailabilityDto dto)
        {
            return;
        }
    }
}
