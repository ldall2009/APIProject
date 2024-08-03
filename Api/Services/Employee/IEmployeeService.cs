using Api.Dtos.Employee;

namespace Api.Services.EmployeeService
{
    public interface IEmployeeService
    {
        Task<GetEmployeeDto?> GetEmployee(int employeeId);
        Task<List<GetEmployeeDto>> GetAllEmployees();
    }
}
