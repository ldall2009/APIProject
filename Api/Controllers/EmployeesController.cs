using Api.Dtos.Employee;
using Api.Models;
using Api.Services.EmployeeService;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeesController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    // ASSUMPTION: Even though the summaries just say to get employee(s), I assume they also want us to fetch their Dependents & map to GetDependentDto.
    // I assumed this because GetEmployeeDto has a ICollection<GetDependentDto> property on it.

    [SwaggerOperation(Summary = "Get employee by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> Get(int id)
    {
        GetEmployeeDto? getEmployeeDto = await _employeeService.GetEmployee(id);

        if (getEmployeeDto == null)
        {
            return NotFound(ApiResponse<GetEmployeeDto>.CreateUnsuccessfulApiResponse(
                "An issue was encountered while trying to get an employee.",
                "No employee was found with the provided id."
                )
            );
        }

        return Ok(ApiResponse<GetEmployeeDto>.CreateSuccessfulApiResponse(getEmployeeDto));
    }

    [SwaggerOperation(Summary = "Get all employees")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetEmployeeDto>>>> GetAll()
    {
        List<GetEmployeeDto> getEmployeeDtos = await _employeeService.GetAllEmployees();
        return Ok(ApiResponse<List<GetEmployeeDto>>.CreateSuccessfulApiResponse(getEmployeeDtos));
    }
}
