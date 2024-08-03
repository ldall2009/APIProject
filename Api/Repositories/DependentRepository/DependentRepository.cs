using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories.DependentRepository
{
    public class DependentRepository : IDependentRepository
    {
        private readonly DatabaseContext _context;

        public DependentRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<Dependent?> GetDependent(int dependentId)
        {
            return await _context.Dependents
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == dependentId);
        }

        public async Task<List<Dependent>> GetAllDependents()
        {
            return await _context.Dependents
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
