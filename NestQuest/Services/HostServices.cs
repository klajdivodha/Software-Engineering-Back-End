using Microsoft.AspNetCore.Mvc;
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
        public Task<int> RemoveProperty(int propertyId);

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
        public Task<int> RemoveProperty(int propertyId)
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
        public Task<int> ChangeEmail(int hostId, string newEmail)
        {
            return;
        }
        public Task<int> ChangePassword(ChangePasswordDto dto)
        {
            return;
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
