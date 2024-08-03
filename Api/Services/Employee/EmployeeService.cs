using Api.Dtos.Employee;
using Api.Extensions.EmployeeExtensions;
using Api.Models;
using Api.Repositories.EmployeeRepository;

namespace Api.Services.EmployeeService
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository) 
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<GetEmployeeDto?> GetEmployee(int employeeId)
        {
            Employee? employee = await _employeeRepository.GetEmployee(employeeId);
            if (employee == null)
            {
                return null;
            }

            GetEmployeeDto getEmployeeDto = employee.ToGetEmployeeDto();
            
            return getEmployeeDto;
        }

        public async Task<List<GetEmployeeDto>> GetAllEmployees()
        {
            List<Employee> employees = await _employeeRepository.GetAllEmployees();
            List<GetEmployeeDto> getEmployeeDtos = employees.Select(s => s.ToGetEmployeeDto()).ToList();

            return getEmployeeDtos;
        }
    }
}
