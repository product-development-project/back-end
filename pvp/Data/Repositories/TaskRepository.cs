using Microsoft.EntityFrameworkCore;
using pvp.Data.Entities;

namespace pvp.Data.Repositories
{
    public interface ITaskRepository
    {
        Task<Uzduotys?> GetAsync(int id);
        Task<IReadOnlyList<Uzduotys>> GetManyAsync();
        Task CreateAsync(Uzduotys Uzduotys);
        Task UpdateAsync(Uzduotys Uzduotys);
        Task DeleteAsync(Uzduotys Uzduotys);
    }



    public class TaskRepository : ITaskRepository
    {
        private readonly SystemDbContext _context;
        public TaskRepository(SystemDbContext context)
        {
            _context = context;
        }

        public async Task<Uzduotys?> GetAsync(int id)
        {
            return await _context.uzduotys.FirstOrDefaultAsync(x => x.id == id);
        }
        public async Task<IReadOnlyList<Uzduotys>> GetManyAsync()
        {
            return await _context.uzduotys.ToListAsync();
        }
        public async Task CreateAsync(Uzduotys Uzduotys)
        {
            _context.uzduotys.Add(Uzduotys);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Uzduotys Uzduotys)
        {
            _context.uzduotys.Update(Uzduotys);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(Uzduotys Uzduotys)
        {
            _context.uzduotys.Remove(Uzduotys);
            await _context.SaveChangesAsync();
        }
    }
}

