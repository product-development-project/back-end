using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using pvp.Data.Entities;

namespace pvp.Data.Repositories
{
    public interface IUserInfoRepositry
    {
        Task<IdentityUser?> GetAsync(string username);
        //Task<IReadOnlyList<IdentityUser>> GetManyAsync();
        Task UpdateAsync(IdentityUser user);
        Task DeleteAsync(IdentityUser user);
    }
    public class UserInfoRepository : IUserInfoRepositry
    {
        private readonly SystemDbContext _context;
        public UserInfoRepository(SystemDbContext context)
        {
            _context = context;
        }
        public async Task<IdentityUser?> GetAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.UserName == username);
        }
        public async Task UpdateAsync(IdentityUser user)
        {
            _context.Users.Update((Auth.RestUsers)user);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(IdentityUser user)
        {
            _context.Users.Remove((Auth.RestUsers)user);
            await _context.SaveChangesAsync();
        }

    }
}
