using Microsoft.EntityFrameworkCore;
using NestQuest.Data;

namespace NestQuest.Services
{
    public interface IAdminServices
    {
        public Task<object> GetAdmin(int id);
        public Task<object[]> GetNotApprovedHosts();
        public Task<int> ApproveHost(int id);
    }
    public class AdminServices : IAdminServices
    {
        private readonly DBContext _context;

        public AdminServices(DBContext context)
        {
            _context = context;
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
    }
}
