using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NestQuest.Data;
using NestQuest.Data.DTO;
using NestQuest.Data.Models;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace NestQuest.Services
{
    public interface IGuestServices
    {
        public Task<object[]> GuestAvaibleProperties(GestAvaiblePropertiesDto dto);
        public Task<ActionResult> PropertieInfo(int id);
    }
    public class GuestServices : IGuestServices
    {
        private readonly DBContext _context;

        public GuestServices(DBContext context)
        {
            _context = context;
        }

        public async Task<object[]> GuestAvaibleProperties(GestAvaiblePropertiesDto dto)
        {
            try
            {
                var properties = await _context.Properties
                                 .Where(p => p.Type == dto.Type &&
                                             p.Max_Nr_Of_Guests >= dto.NrOfGuests &&
                                             p.City == dto.City &&
                                             p.Country == dto.Country)
                                             .Select(p => new
                                             {
                                                 p.Name,
                                                 p.Daily_Price,
                                                 p.Address,
                                                 p.Property_ID,
                                                 p.Overall_Rating,
                                                 p.Preium_Fee_Start,
                                                 p.Type,
                                                 p.City,
                                                 p.Country,
                                             })   
                                             .ToArrayAsync();

                var notAvailableIds = await _context.Bookings
                    .Where(b => properties.Select(prop => prop.Property_ID).Contains(b.Property_Id) &&
                           b.Status== "upcomming" && 
                           ((dto.StratDate >= b.Start_Date && dto.StratDate <= b.End_Date) ||
                            (dto.EndDate >= b.Start_Date && dto.EndDate <= b.End_Date)))
                    .Select(b => b.Property_Id)
                    .ToListAsync();

                var availableProperties = properties.Where(prop => !notAvailableIds.Contains(prop.Property_ID)).ToArray();

                return availableProperties;
                if (availableProperties.Any())
                {
                    return availableProperties;
                }
                return [];
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ActionResult> PropertieInfo(int id)
        {
            try
            {
                var result= await _context.Properties
                            .Where(p=> p.Property_ID == id)
                            .Include(p => p.Utilities)
                            .Include(p => p.Beds)
                            .Include(p => p.Reviews)
                            .FirstOrDefaultAsync();

                if (result == null)
                {
                    return null;
                }
                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve
                };

                var serializedResult = JsonSerializer.Serialize(result, options);

                return new JsonResult(serializedResult);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
