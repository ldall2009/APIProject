using Api.Dtos.Employee;
using Api.Dtos.Paycheck;
using Api.Services.Paycheck.Calculators;

namespace Api.Services.PaycheckService
{
    public class PaycheckService : IPaycheckService
    {
        private readonly IPaycheckCalculator _paycheckCalculator;

        public PaycheckService(IPaycheckCalculator paycheckCalculator) 
        {
            _paycheckCalculator = paycheckCalculator;
        }

        // ASSUMPTION: We would not want to use the Employee entity directly, but rather, a Dto.
        // Since the GetEmployeeDto was already provided, and contains all fields we would need, we are using that here.
        // In the event this is unwanted, we could either have the employeeService have a method that just returns an Employee,
        // or we could pass an employeeId into this method & inject the employeeRepository into this class as well.
        public GetPaycheckDto GetPaycheck(GetEmployeeDto getEmployeeDto)
        {
            if (getEmployeeDto == null) 
            {
                throw new ArgumentNullException(nameof(getEmployeeDto));
            }

            decimal earningsPerPaycheck = _paycheckCalculator.CalculateEarningsPerPaycheck(getEmployeeDto);
            ICollection<GetPaycheckDeductionDto> paycheckDeductionDtos = _paycheckCalculator.CalculatePaycheckDeductionDtos(getEmployeeDto);

            return new GetPaycheckDto()
            {
                EmployeeName = $"{getEmployeeDto.FirstName} {getEmployeeDto.LastName}",
                TotalEarnings = earningsPerPaycheck,
                Deductions = paycheckDeductionDtos,
            };
        }
    }
}
