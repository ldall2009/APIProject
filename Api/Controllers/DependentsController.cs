using Api.Dtos.Dependent;
using Api.Models;
using Api.Services.DependentService;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class DependentsController : ControllerBase
{
    private readonly IDependentService _dependentService;

    public DependentsController(IDependentService dependentService) 
    {
        _dependentService = dependentService;
    }

    [SwaggerOperation(Summary = "Get dependent by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetDependentDto>>> Get(int id)
    {
        GetDependentDto? getDependentDto = await _dependentService.GetDependent(id);

        if(getDependentDto == null)
        {
            return NotFound(ApiResponse<GetDependentDto>.CreateUnsuccessfulApiResponse(
                "An issue was encountered while trying to get a dependent.",
                "No dependent was found with the provided id."
                )
            );
        }

        return Ok(ApiResponse<GetDependentDto>.CreateSuccessfulApiResponse(getDependentDto));
    }

    [SwaggerOperation(Summary = "Get all dependents")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetDependentDto>>>> GetAll()
    {
        List<GetDependentDto> getDependentDtos = await _dependentService.GetAllDependents();
        return Ok(ApiResponse<List<GetDependentDto>>.CreateSuccessfulApiResponse(getDependentDtos));
    }
}
