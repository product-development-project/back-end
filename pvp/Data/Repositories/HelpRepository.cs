using Microsoft.EntityFrameworkCore;
using pvp.Data.Entities;

namespace pvp.Data.Repositories
{
    public interface IHelpRepository
    {
        Task<Help?> GetAsync(int id);
        Task<IReadOnlyList<Help>> GetManyAsync();
        Task CreateAsync(Help Skelbimas);
        Task UpdateAsync(Help Skelbimas);
        Task DeleteAsync(Help Skelbimas);
    }



    public class HelpRepository : IHelpRepository
    {
        private readonly SystemDbContext _context;
        public HelpRepository(SystemDbContext context)
        {
            _context = context;
        }

        public async Task<Help?> GetAsync(int id)
        {
            return await _context.help.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<IReadOnlyList<Help>> GetManyAsync()
        {
            return await _context.help.ToListAsync();
        }
        public async Task CreateAsync(Help Skelbimas)
        {
            _context.help.Add(Skelbimas);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Help Skelbimas)
        {
            _context.help.Update(Skelbimas);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(Help Skelbimas)
        {
            _context.help.Remove(Skelbimas);
            await _context.SaveChangesAsync();
        }
    }
}
