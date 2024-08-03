using Api.Models;

namespace Api.Repositories.EmployeeRepository
{
    public interface IEmployeeRepository
    {
        Task<Employee?> GetEmployee(int employeeId);
        Task<List<Employee>> GetAllEmployees();
    }
}
