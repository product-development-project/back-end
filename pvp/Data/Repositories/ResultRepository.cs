using Microsoft.EntityFrameworkCore;
using pvp.Data.Entities;

namespace pvp.Data.Repositories
{
        public interface IResultRepository
        {
            Task<Rezultatai?> GetAsync(int id);
            Task<IReadOnlyList<Rezultatai>> GetManyAsync();
            Task CreateAsync(Rezultatai Rezultatai);
            Task UpdateAsync(Rezultatai Rezultatai);
            Task DeleteAsync(Rezultatai Rezultatai);
        }



        public class ResultRepository :IResultRepository
        {
            private readonly SystemDbContext _context;
            public ResultRepository(SystemDbContext context)
            {
                _context = context;
            }

            public async Task<Rezultatai?> GetAsync(int id)
            {
                return await _context.rezultatais.FirstOrDefaultAsync(x => x.Id == id);
            }
            public async Task<IReadOnlyList<Rezultatai>> GetManyAsync()
            {
                return await _context.rezultatais.ToListAsync();
            }
            public async Task CreateAsync(Rezultatai Rezultatai)
            {
                _context.rezultatais.Add(Rezultatai);
                await _context.SaveChangesAsync();
            }
            public async Task UpdateAsync(Rezultatai Rezultatai)
            {
                _context.rezultatais.Update(Rezultatai);
                await _context.SaveChangesAsync();
            }
            public async Task DeleteAsync(Rezultatai Rezultatai)
            {
                _context.rezultatais.Remove(Rezultatai);
                await _context.SaveChangesAsync();
            }
        }
 }
