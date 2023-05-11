using Microsoft.EntityFrameworkCore;
using pvp.Data.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace pvp.Data.Repositories
{
    public interface ICompanyRepository
    {
        Task<Kompanija?> GetAsync(string userId);
        Task<IReadOnlyList<Kompanija>> GetManyAsync();
        Task CreateAsync(Kompanija Kompanija);
        Task UpdateAsync(Kompanija Kompanija);
        Task DeleteAsync(Kompanija Kompanija);
    }

    public class CompanyRepository : ICompanyRepository
    {
        private readonly SystemDbContext _context;
        public CompanyRepository(SystemDbContext context)
        {
            _context = context;
        }

        public async Task<Kompanija?> GetAsync(string userId)
        {
            return await _context.kompanija.FirstOrDefaultAsync(x => x.UserId == userId);
        }
        public async Task<IReadOnlyList<Kompanija>> GetManyAsync()
        {
            return await _context.kompanija.ToListAsync();
        }
        public async Task CreateAsync(Kompanija Kompanija)
        {
            _context.kompanija.Add(Kompanija);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Kompanija Kompanija)
        {
            _context.kompanija.Update(Kompanija);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(Kompanija Kompanija)
        {
            _context.kompanija.Remove(Kompanija);
            await _context.SaveChangesAsync();
        }
    }
}
