using Api.Dtos.Employee;
using Api.Extensions.DependentExtensions;
using Api.Models;

namespace Api.Extensions.EmployeeExtensions
{
    public static class EmployeeExtensions
    {
        // This method assumes we always want to map Dependents as well.
        // In the event this assumption was false, we could create a separate ToGetEmployeeDtoWithDependents method, which would populate that collection.
        public static GetEmployeeDto ToGetEmployeeDto(this Employee employee)
        {
            return new()
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Salary = employee.Salary,
                DateOfBirth = employee.DateOfBirth,
                Dependents = employee.Dependents.Select(s => s.ToGetDependentDto()).ToList(),
            };
        }
        
        // Could create a ToEmployeeEntity extension method here if we had Create/Update Employee endpoints.
    }
}
