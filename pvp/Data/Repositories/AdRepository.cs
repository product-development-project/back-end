using Microsoft.EntityFrameworkCore;
using pvp.Data.Entities;

namespace pvp.Data.Repositories
{
        public interface IAdRepository
        {
            Task<Skelbimas?> GetAsync(int id);
            Task<IReadOnlyList<Skelbimas>> GetManyAsync();
            Task CreateAsync(Skelbimas Skelbimas);
            Task UpdateAsync(Skelbimas Skelbimas);
            Task DeleteAsync(Skelbimas Skelbimas);
        }



        public class AdRepository :IAdRepository
        {
            private readonly SystemDbContext _context;
            public AdRepository(SystemDbContext context)
            {
                _context = context;
            }

            public async Task<Skelbimas?> GetAsync(int id)
            {
                return await _context.skelbimas.FirstOrDefaultAsync(x => x.id == id);
            }
            public async Task<IReadOnlyList<Skelbimas>> GetManyAsync()
            {
                return await _context.skelbimas.ToListAsync();
            }
            public async Task CreateAsync(Skelbimas Skelbimas)
            {
                _context.skelbimas.Add(Skelbimas);
                await _context.SaveChangesAsync();
            }
            public async Task UpdateAsync(Skelbimas Skelbimas)
            {
                _context.skelbimas.Update(Skelbimas);
                await _context.SaveChangesAsync();
            }
            public async Task DeleteAsync(Skelbimas Skelbimas)
            {
                _context.skelbimas.Remove(Skelbimas);
                await _context.SaveChangesAsync();
            }
        }
}
