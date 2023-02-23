using Microsoft.EntityFrameworkCore;
using pvp.Data.Entities;

namespace pvp.Data.Repositories
{
        public interface ILoggedRepository
        {
            Task<Prisijunge?> GetAsync(int id);
            Task<IReadOnlyList<Prisijunge>> GetManyAsync();
            Task CreateAsync(Prisijunge Prisijunge);
            Task UpdateAsync(Prisijunge Prisijunge);
            Task DeleteAsync(Prisijunge Prisijunge);
        }



        public class LoggedRepository : ILoggedRepository
        {
            private readonly SystemDbContext _context;
            public LoggedRepository(SystemDbContext context)
            {
                _context = context;
            }

            public async Task<Prisijunge?> GetAsync(int id)
            {
                return await _context.prisijunges.FirstOrDefaultAsync(x => x.Id == id);
            }
            public async Task<IReadOnlyList<Prisijunge>> GetManyAsync()
            {
                return await _context.prisijunges.ToListAsync();
            }
            public async Task CreateAsync(Prisijunge Prisijunge)
            {
                _context.prisijunges.Add(Prisijunge);
                await _context.SaveChangesAsync();
            }
            public async Task UpdateAsync(Prisijunge Prisijunge)
            {
                _context.prisijunges.Update(Prisijunge);
                await _context.SaveChangesAsync();
            }
            public async Task DeleteAsync(Prisijunge Prisijunge)
            {
                _context.prisijunges.Remove(Prisijunge);
                await _context.SaveChangesAsync();
            }
        }
}
