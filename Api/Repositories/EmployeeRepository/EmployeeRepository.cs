using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories.EmployeeRepository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DatabaseContext _context;

        public EmployeeRepository(DatabaseContext context) 
        {
            _context = context;
        }

        // Current requirements want us to include dependents as well, but if we eventually needed to JUST fetch employees (without dependents),
        // we could change these existing methods to be something like "GetEmployeeIncludingDependents" (basically provide a suffix to indicate what we include).
        public async Task<Employee?> GetEmployee(int employeeId)
        {
            return await _context.Employees
                .Include(i => i.Dependents)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == employeeId);
        }

        // ASSUMPTION: Returning List<T> rather than something like IEnumerable<T> is okay here since EF Core needs to convert it to some collection
        // (I chose ToListAsync in this case).  If we wanted to aim for better abstraction though, we could return something like IEnumerable<T> instead.
        public async Task<List<Employee>> GetAllEmployees()
        {
            return await _context.Employees
                .Include(i => i.Dependents)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
