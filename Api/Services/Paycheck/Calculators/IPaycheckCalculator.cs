using Api.Dtos.Employee;
using Api.Dtos.Paycheck;

namespace Api.Services.Paycheck.Calculators
{
    public interface IPaycheckCalculator
    {
        decimal CalculateEarningsPerPaycheck(GetEmployeeDto getEmployeeDto);
        ICollection<GetPaycheckDeductionDto> CalculatePaycheckDeductionDtos(GetEmployeeDto getEmployeeDto);
        GetPaycheckDeductionDto CalculateBaseBenefitsDeduction();
        GetPaycheckDeductionDto CalculateDependentsCountDeduction(GetEmployeeDto getEmployeeDto);
        GetPaycheckDeductionDto CalculateDependentsOverAgeThresholdCountDeduction(GetEmployeeDto getEmployeeDto);
        GetPaycheckDeductionDto CalculateHighSalaryDeduction(GetEmployeeDto getEmployeeDto);
    }
}
