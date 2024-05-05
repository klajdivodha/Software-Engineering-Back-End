using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using NestQuest.Data;
using NestQuest.Data.DTO;
using NestQuest.Data.Models;
using System;
using System.Security.Cryptography;

namespace NestQuest.Services
{
    public interface ISignUpServices
    {
        Task<Users> SignUpHost(SignUpHostDto hostDto);
        Task<Users> SignUpGuest(SignUpGuestDto userDto);
    }
    public class SignUpServices : ISignUpServices
    {
        private readonly DBContext _context;

        public SignUpServices(DBContext context)
        {
            _context = context;
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

        public async Task<Users> SignUpGuest(SignUpGuestDto userDto)
        {

            try
            {
                var condition = await _context.Users.Where(e => e.Email == userDto.Email).FirstOrDefaultAsync();
                if (condition != null)
                {
                    return null;
                }
                Users newUser = new Users
                {
                    Name = userDto.Name,
                    Surname = userDto.Surname,
                    Email = userDto.Email,
                    Password = HashPassword(userDto.Password),
                    Phone = userDto.Phone,
                    Birthday = userDto.Birthday,
                    UserType = "guest",
                    Two_Fa = userDto.Two_Fa,
                    Nationality = userDto.Nationality,
                };

                await _context.Users.AddAsync(newUser);
                await _context.SaveChangesAsync();

                Guest guest = new Guest
                {
                    Guest_Id = newUser.User_Id,
                    rating = 5.0,
                    Nr_Of_Ratings = 1,
                    banned = false,
                };
                await _context.Guest.AddAsync(guest);
                await _context.SaveChangesAsync();
                return newUser;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task<Users> SignUpHost(SignUpHostDto hostDto)
        {
            try
            {
                var condition = await _context.Users.Where(e => e.Email == hostDto.Email).FirstOrDefaultAsync();
                if (condition!=null) {
                    return null;
                }
                Users newUser = new Users
                {
                    Name = hostDto.Name,
                    Surname = hostDto.Surname,
                    Email = hostDto.Email,
                    Password = HashPassword(hostDto.Password),
                    Phone = hostDto.Phone,
                    Birthday = hostDto.Birthday,
                    UserType = "host",
                    Two_Fa = hostDto.Two_Fa,
                    Nationality = hostDto.Nationality,
                };

                await _context.Users.AddAsync(newUser);
                await _context.SaveChangesAsync();

                Hosts host = new Hosts
                {
                    Host_Id = newUser.User_Id,
                    aproved = false,
                    banned=false,
                    startDate=DateTime.UtcNow,
                    Nr_Of_Ratings = 1,
                    rating =5.0,

            };
                await _context.Host.AddAsync(host);
                await _context.SaveChangesAsync();
                return newUser;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
