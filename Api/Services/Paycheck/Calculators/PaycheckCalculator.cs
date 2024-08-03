using Api.Constants;
using Api.Dtos.Employee;
using Api.Dtos.Paycheck;
using Api.Extensions.DecimalExtensions;
using Api.Helpers;

namespace Api.Services.Paycheck.Calculators
{
    public class PaycheckCalculator : IPaycheckCalculator
    {
        public PaycheckCalculator() 
        {

        }

        // These 2 private methods could handle the cases if particular data is invalid.  Given the original Employee/Dependent data provided in the skeleton of this project,
        // the number values provided on the document, and the endpoints the project currently has, I chose to just have these methods throw a specific exception if
        // they contain invalid data.  I did this for the sake of acknowledging edge cases, and writing unit tests that make sure we encounter these
        // exceptions when we provide bad data.
        private void EnsureGetEmployeeDtoIsNotNull(GetEmployeeDto getEmployeeDto)
        {
            if (getEmployeeDto == null)
            {
                throw new ArgumentNullException(nameof(getEmployeeDto));
            }
        }

        private void EnsureDenominatorIsNotZero(int denominator)
        {
            if (denominator == 0)
            {
                throw new ArgumentException(nameof(denominator));
            }
        }

        public decimal CalculateEarningsPerPaycheck(GetEmployeeDto getEmployeeDto)
        {
            EnsureGetEmployeeDtoIsNotNull(getEmployeeDto);
            EnsureDenominatorIsNotZero(PaycheckConstants.PaychecksPerYear);

            // ASSUMPTION: Salary field on getEmployeeDto is their Annual salary.
            decimal earningsPerPaycheck = getEmployeeDto.Salary / PaycheckConstants.PaychecksPerYear;

            // ASSUMPTION: Assume we want to round to nearest cent.
            return earningsPerPaycheck.RoundToNearestHundreths();
        }

        public ICollection<GetPaycheckDeductionDto> CalculatePaycheckDeductionDtos(GetEmployeeDto getEmployeeDto)
        {
            ICollection<GetPaycheckDeductionDto> paycheckDeductions = new List<GetPaycheckDeductionDto>
            {
                CalculateBaseBenefitsDeduction(),
                CalculateDependentsCountDeduction(getEmployeeDto),
                CalculateDependentsOverAgeThresholdCountDeduction(getEmployeeDto),
                CalculateHighSalaryDeduction(getEmployeeDto),
            };

            // ASSUMPTION: Assume we do not care to return deductions that are just equal to zero.
            return paycheckDeductions
                .Where(w => w != null && w.Amount != 0)
                .ToList();
        }

        public GetPaycheckDeductionDto CalculateBaseBenefitsDeduction()
        {
            EnsureDenominatorIsNotZero(PaycheckConstants.PaychecksPerYear);

            decimal baseBenefitsCostPerPaycheck = PaycheckConstants.BaseBenefitsCostPerYear / PaycheckConstants.PaychecksPerYear;

            return CreatePerPaycheckDeduction("Base Benefits Cost", baseBenefitsCostPerPaycheck);
        }

        public GetPaycheckDeductionDto CalculateDependentsCountDeduction(GetEmployeeDto getEmployeeDto)
        {
            EnsureGetEmployeeDtoIsNotNull(getEmployeeDto);
            EnsureDenominatorIsNotZero(PaycheckConstants.PaychecksPerYear);

            decimal costOfDependentsPerYear = getEmployeeDto.Dependents.Count * PaycheckConstants.DependentsBenefitsCostPerYear;
            decimal costOfDependentsPerPaycheck = costOfDependentsPerYear / PaycheckConstants.PaychecksPerYear;

            return CreatePerPaycheckDeduction("Benefits Cost for Number of Dependents", costOfDependentsPerPaycheck);
        }

        public GetPaycheckDeductionDto CalculateDependentsOverAgeThresholdCountDeduction(GetEmployeeDto getEmployeeDto)
        {
            EnsureGetEmployeeDtoIsNotNull(getEmployeeDto);
            EnsureDenominatorIsNotZero(PaycheckConstants.PaychecksPerYear);

            int numberOfDependentsOverAgeThreshold = getEmployeeDto.Dependents
                .Count(c => AgeHelper.GetAge(c.DateOfBirth) > PaycheckConstants.AgeThresholdForAdditionalCost);

            decimal costOfDependentsOverAgeThresholdPerYear = numberOfDependentsOverAgeThreshold * PaycheckConstants.DependentsAboveAgeThresholdBenefitsCostPerYear;
            decimal costOfDependentsOverAgeThresholdPerPaycheck = costOfDependentsOverAgeThresholdPerYear / PaycheckConstants.PaychecksPerYear;

            return CreatePerPaycheckDeduction(PaycheckConstants.DependentsAboveAgeThresholdBenefitsCostLabel, costOfDependentsOverAgeThresholdPerPaycheck);
        }

        public GetPaycheckDeductionDto CalculateHighSalaryDeduction(GetEmployeeDto getEmployeeDto)
        {
            EnsureGetEmployeeDtoIsNotNull(getEmployeeDto);
            EnsureDenominatorIsNotZero(PaycheckConstants.PaychecksPerYear);

            // "employees that make more than $80,000 per year will incur an additional 2% of their yearly salary in benefits costs."
            // ASSUMPTION: The 2% would be applied over the course of the year, so we divide by the number of paychecks per year.
            decimal highSalaryCost = getEmployeeDto.Salary > PaycheckConstants.SalaryThresholdForAdditionalBenefits
                ? getEmployeeDto.Salary * PaycheckConstants.HighSalaryBenefitsMultiplier / PaycheckConstants.PaychecksPerYear
                : 0;

            return CreatePerPaycheckDeduction(PaycheckConstants.HighSalaryBenefitsCostLabel, highSalaryCost);
        }

        private GetPaycheckDeductionDto CreatePerPaycheckDeduction(string label, decimal deductionPerPaycheck)
        {
            return new()
            {
                Label = label,
                // ASSUMPTION: Assume we just want to round to the nearest cent for these costs.
                Amount = deductionPerPaycheck.RoundToNearestHundreths(),
            };
        }
    }
}
