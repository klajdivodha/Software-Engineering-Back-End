using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NestQuest.Data;
using NestQuest.Data.DTO;
using NestQuest.Data.Models;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace NestQuest.Services
{
    public interface IGuestServices
    {
        public Task<object[]> GuestAvaibleProperties(GestAvaiblePropertiesDto dto);
        public Task<ActionResult> PropertieInfo(int id);

        public Task<object> GetGuest(int id);

        public Task<int> ChangeEmail(int id, string email);

        public Task<int> ChangePassword(ChangePasswordDto dto);
    }
    public class GuestServices : IGuestServices
    {
        private readonly DBContext _context;

        public GuestServices(DBContext context)
        {
            _context = context;
        }
        private bool VerifyPassword(string storedHash, string providedPassword)
        {
            // Split the stored hash to get the salt and the hash components
            var parts = storedHash.Split(':', 2);
            if (parts.Length != 2)
            {
                throw new FormatException("The stored password hash is not in the expected format.");
            }

            var salt = Convert.FromBase64String(parts[0]);
            var storedSubkey = parts[1];

            // Hash the provided password using the same salt
            string hashedProvidedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: providedPassword,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            // Compare the hashes
            return storedSubkey == hashedProvidedPassword;
        }
        private string HashPassword(string password)
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            return $"{Convert.ToBase64String(salt)}:{hashed}";
        }

        public async Task<int> ChangeEmail(int id, string email)
        {
            try
            {
                var result = await _context.Users
                            .Where(u => u.User_Id == id)
                            .FirstOrDefaultAsync();

                if (result == null) { return -1; }
                var condition = await _context.Users.Where(e => e.Email == email).FirstOrDefaultAsync();
                if (condition != null)
                {
                    return -2;
                }
                result.Email = email;

                var nr=await _context.SaveChangesAsync();
                return nr;

            }
            catch(Exception ex)
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
                if(VerifyPassword(rezult.Password, dto.Password))
                {
                    rezult.Password = HashPassword(dto.NewPassword);

                    var nr=_context.SaveChanges();
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

        public async Task<object> GetGuest(int id)
        {
            try
            {
                var guest = await _context.Users
                    .Where(g => g.User_Id == id)
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
                    })
                    .FirstOrDefaultAsync();

                if (guest == null)
                {
                    return null;
                }
                return guest;
            }
            catch (Exception ex)
            {
                throw;
            }
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
