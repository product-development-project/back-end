using Microsoft.EntityFrameworkCore;
using pvp.Data.Entities;

namespace pvp.Data.Repositories
{
    public interface ITypeRepository
    {
        Task<Tipas?> GetAsync(int id);
        Task<IReadOnlyList<Tipas>> GetManyAsync();
        Task CreateAsync(Tipas Tipas);
        Task UpdateAsync(Tipas Tipas);
        Task DeleteAsync(Tipas Tipas);
    }



    public class TypeRepository : ITypeRepository
    {
        private readonly SystemDbContext _context;
        public TypeRepository(SystemDbContext context)
        {
            _context = context;
        }

        public async Task<Tipas?> GetAsync(int id)
        {
            return await _context.tipas.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<IReadOnlyList<Tipas>> GetManyAsync()
        {
            return await _context.tipas.ToListAsync();
        }
        public async Task CreateAsync(Tipas Tipas)
        {
            _context.tipas.Add(Tipas);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Tipas Tipas)
        {
            _context.tipas.Update(Tipas);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(Tipas Tipas)
        {
            _context.tipas.Remove(Tipas);
            await _context.SaveChangesAsync();
        }
    }
}
}
