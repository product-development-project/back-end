using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Tsp;
using pvp.Data.Entities;

namespace pvp.Data.Repositories
{
    public interface ISolutionRepository
    {
        Task<Sprendimas?> GetAsyncByTaskId(int id);
        Task<Sprendimas?> GetAsyncBySolutionId(int id);
        Task<Sprendimas?> GetAsyncByUserIdAndTaskId(int userId, int taskId);
        Task<IReadOnlyList<Sprendimas>> GetManyAsyncByUserId(int id);
        Task<IReadOnlyList<Sprendimas>> GetManyAsync();
        Task CreateAsync(Sprendimas Sprendimas);
        Task UpdateAsync(Sprendimas Sprendimas);
        Task DeleteAsync(Sprendimas Sprendimas);
    }

    public class SolutionRepository : ISolutionRepository
    {
        private readonly SystemDbContext _context;
        public SolutionRepository(SystemDbContext context)
        {
            _context = context;
        }

        public async Task<Sprendimas?> GetAsyncBySolutionId(int id)
        {
            return await _context.sprendimas.FirstOrDefaultAsync(x => x.id == id);
        }

        public async Task<Sprendimas?> GetAsyncByUserIdAndTaskId(int userId, int taskId)
        {
            return await _context.sprendimas.FirstOrDefaultAsync(x => x.Prisijunge_id == userId && x.ParinktosUzduotys_id == taskId);
        }

        public async Task<Sprendimas?> GetAsyncByTaskId(int id)
        {
            return await _context.sprendimas.FirstOrDefaultAsync(x => x.id == id);
        }

        public async Task<IReadOnlyList<Sprendimas>> GetManyAsyncByUserId(int id)
        {
            var sol = await _context.sprendimas
                .Where(s => s.Prisijunge_id == id)
                .ToListAsync();

            return sol;
        }

        public async Task<IReadOnlyList<Sprendimas>> GetManyAsync()
        {
            var sol = await _context.sprendimas.ToListAsync();
            return sol;
        }

        public async Task CreateAsync(Sprendimas Sprendimas)
        {
            _context.sprendimas.Add(Sprendimas);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Sprendimas Sprendimas)
        {
            _context.sprendimas.Update(Sprendimas);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Sprendimas Sprendimas)
        {
            _context.sprendimas.Remove(Sprendimas);
            await _context.SaveChangesAsync();
        }

    }
}

