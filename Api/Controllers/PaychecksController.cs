using Api.Dtos.Employee;
using Api.Dtos.Paycheck;
using Api.Models;
using Api.Services.EmployeeService;
using Api.Services.PaycheckService;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class PaychecksController : ControllerBase
{
    private readonly IPaycheckService _paycheckService;
    private readonly IEmployeeService _employeeService;

    public PaychecksController(IPaycheckService paycheckService,
        IEmployeeService employeeService)
    {
        _paycheckService = paycheckService;
        _employeeService = employeeService;
    }

    [SwaggerOperation(Summary = "Get paycheck by employee id")]
    [HttpGet("employee/{employeeId:int}")]
    public async Task<ActionResult<ApiResponse<GetPaycheckDto>>> GetPaycheckByEmployeeId(int employeeId)
    {
        GetEmployeeDto? getEmployeeDto = await _employeeService.GetEmployee(employeeId);

        if (getEmployeeDto == null)
        {
            return NotFound(ApiResponse<GetPaycheckDto>.CreateUnsuccessfulApiResponse(
                "An issue was encountered while trying to get a paycheck.",
                "No employee was found with the provided id."
                )
            );
        }
        GetPaycheckDto getPaycheckDto = _paycheckService.GetPaycheck(getEmployeeDto);
        
        return Ok(ApiResponse<GetPaycheckDto>.CreateSuccessfulApiResponse(getPaycheckDto));
    }
}