﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NestQuest.Data;
using NestQuest.Data.DTO;
using NestQuest.Data.Models;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System.Linq;

namespace NestQuest.Services
{
    public interface IGuestServices
    {
        public Task<object[]> GuestAvaibleProperties(GestAvaiblePropertiesDto dto);
        public Task<ActionResult> PropertieInfo(int id);

        public Task<object> GetGuest(int id);

        public Task<int> ChangeEmail(int id, string email);

        public Task<int> ChangePassword(ChangePasswordDto dto);

        public Task<int> AddFavorites(int user_id,int property_id);

        public Task<int> DeleteFavorites(int user_id,int property_id);

        public Task<object[]> GetFavorites(int id);

        public Task<bool> CheckAvailability(CheckAvailabilityDto dto);

        public Task<int> AddBooking(Bookings obj);

        public Task<int> CancelBooking(BookingDto dto);

        public Task<object[]> GetBookings(int id);

        public Task<int> AddReview(AddReviewDto dto);

        public Task<int> AddReporting(Reportings obj);

        public Task<int> AddRatings(AddRatingsDto dto);

        public Task<int> RateHost(int id, double rating);
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
                                                 p.Nr_Of_Ratings,
                                                 p.Preium_Fee_Start,
                                                 p.Type,
                                                 p.City,
                                                 p.Country,
                                             })   
                                             .ToArrayAsync();

                var notAvailableIds = await _context.Bookings
                    .Where(b => properties.Select(prop => prop.Property_ID).Contains(b.Property_Id) &&
                           b.Status== "upcoming" && 
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

        public async Task<int> AddFavorites(int user_id, int property_id)
        {
            try
            {
                Favorites obj=new Favorites();
                obj.Property_Id = property_id;
                obj.Guest_Id= user_id;

                await _context.Favorites.AddAsync(obj);
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex) 
            {
                throw;
            }
        }

        public async Task<int> DeleteFavorites(int user_id, int property_id)
        {
            try
            {
                var rezult= await _context.Favorites
                            .Where(f=> f.Property_Id == property_id && f.Guest_Id== user_id)
                            .FirstOrDefaultAsync();
                if(rezult == null) { return -1; }
                _context.Favorites.Remove(rezult);
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<object[]> GetFavorites(int id)
        {
            try
            {
                var rezult= await _context.Favorites
                        .Where(f=> f.Guest_Id==id)
                        .Select(f=> f.Property_Id)
                        .ToArrayAsync();

                if(rezult == null) { return []; }

                var rez= await _context.Properties
                    .Where(p => rezult.Contains(p.Property_ID))
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

                return rez;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> CheckAvailability(CheckAvailabilityDto dto)
        {
            try
            {
                var result = await _context.Bookings
                    .Where(b => b.Property_Id == dto.Property_Id &&
                                 b.Status == "upcoming" &&
                                ((dto.StartDate >= b.Start_Date && dto.StartDate <= b.End_Date) ||
                                 (dto.EndDate >= b.Start_Date && dto.EndDate <= b.End_Date)))
                    .FirstOrDefaultAsync();

                if (result != null) { return false; }

                var rez = await _context.Properties
                        .Where(p => p.Property_ID == dto.Property_Id && p.Max_Nr_Of_Guests >= dto.NrOfGuests)
                        .FirstOrDefaultAsync();
                if (rez == null) { return false; }
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<int> AddBooking(Bookings obj)
        {
            try
            {
                await _context.Bookings.AddAsync(obj);
                var result= await _context.SaveChangesAsync();

                var rez=await _context.Properties
                    .Where(p=> p.Property_ID==obj.Property_Id)
                    .FirstOrDefaultAsync();

                if(rez != null)
                {
                    rez.Nr_Of_Bookings += 1;
                    _context.SaveChangesAsync();
                }


                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<int> CancelBooking(BookingDto dto)
        {
            try
            {
                var result= await _context.Bookings
                        .Where(b=> b.Property_Id == dto.Property_Id && b.Start_Date == dto.StartDate && b.BookingTime==dto.BookingTime)
                        .FirstOrDefaultAsync();

                if (result == null) { return -1; }
                result.Status="canceled"; 
                return await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<object[]> GetBookings(int id)
        {
            try
            {
                var result = await _context.Bookings
                    .Where(b => b.Guest_Id == id)
                    .ToArrayAsync();

                if (result == null) { return [] ; }
                return result;

            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<int> AddReview(AddReviewDto dto)
        {
            try
            {
                Reviews rev= new Reviews();
                rev.Property_ID=dto.Property_id;
                rev.Description = dto.Review;
                
                await _context.Reviews.AddAsync(rev);
                return await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<int> AddReporting(Reportings obj)
        {
            try
            {
                await _context.Reportings.AddAsync(obj);
                return await _context.SaveChangesAsync();
            }
            catch( Exception ex)
            {
                throw;
            }
        }

        public async Task<int> AddRatings(AddRatingsDto dto)
        {
            try
            {
                var rezult = await _context.Properties
                        .Where(p => p.Property_ID == dto.Property_Id)
                        .FirstOrDefaultAsync();
                if(rezult == null) { return 0 ; }
                rezult.Checkin_Rating += dto.Checkin_Rating;
                rezult.Cleanliness_Rating += dto.Cleanliness_Rating;
                rezult.Accuracy_Rating += dto.Accuracy_Rating;
                rezult.Location_Rating += dto.Location_Rating;    
                rezult.Communication_Rating=dto.Communication_Rating;
                rezult.Price_Rating += dto.Price_Rating;
                rezult.Nr_Of_Ratings += 1;
                var nr = rezult.Nr_Of_Ratings;
                rezult.Overall_Rating = (rezult.Overall_Rating + (rezult.Checkin_Rating / nr) + (rezult.Cleanliness_Rating / nr) + (rezult.Accuracy_Rating / nr) + (rezult.Location_Rating / nr) + (rezult.Communication_Rating / nr) + (rezult.Price_Rating / nr)) /6;

                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<int> RateHost(int id,double rating)
        {
            try
            {
                var result= await _context.Host.Where(h=> h.Host_Id == id).FirstOrDefaultAsync();

                result.rating += rating;
                result.Nr_Of_Ratings += 1;
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}