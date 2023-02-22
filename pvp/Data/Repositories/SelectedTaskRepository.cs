using Microsoft.EntityFrameworkCore;
using pvp.Data.Entities;

namespace pvp.Data.Repositories
{
    public interface ISelectedTaskRepository
    {

    }



    public class SelectedTaskRepository
    {
        private readonly SystemDbContext _context;
        public SelectedTaskRepository(SystemDbContext context) 
        {
            _context = context;
        }

        public async Task<ParinktosUzduotys?> GetAsync (int id)
        {
            return await _context.parinktosUzduotys.FirstOrDefaultAsync (x => x.id == id);
        }
        public async Task<IReadOnlyList<ParinktosUzduotys>> GetManyAsync()
        {
            return await _context.parinktosUzduotys.ToListAsync();
        }
        public async Task CreateAsync(ParinktosUzduotys parinktosUzduotys)
        {
            _context.parinktosUzduotys.Add(parinktosUzduotys);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(ParinktosUzduotys parinktosUzduotys)
        {
            _context.parinktosUzduotys.Update(parinktosUzduotys);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(ParinktosUzduotys parinktosUzduotys)
        {
            _context.parinktosUzduotys.Remove(parinktosUzduotys);
            await _context.SaveChangesAsync();
        }
    }
}
