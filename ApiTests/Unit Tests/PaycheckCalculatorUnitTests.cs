using Api.Constants;
using Api.Dtos.Employee;
using Api.Dtos.Paycheck;
using Api.Services.Paycheck.Calculators;
using System.Collections.Generic;
using System;
using Xunit;
using Api.Dtos.Dependent;
using Api.Extensions.DecimalExtensions;
using System.Linq;
using Api.Helpers;
using NuGet.Frameworks;

namespace ApiTests.Unit_Tests
{
    // The methods in this class will follow the Arrange, Act, Assert model of unit testing.

    // The document also just said "test coverage for the cost calculations."  As such, I believe this is the only unit test class I was expected to create,
    // and it wanted me to just write unit tests for each method that calculates some specific type of paycheck cost.
    public class PaycheckCalculatorUnitTests
    {
        private readonly IPaycheckCalculator _paycheckCalculator;

        public PaycheckCalculatorUnitTests()
        {
            _paycheckCalculator = new PaycheckCalculator();
        }

        [Fact]
        public void CalculateEarningsPerPaycheck_ForEmployee_ShouldReturnCorrectEarnings()
        {
            GetEmployeeDto getEmployeeDto = new()
            {
                Salary = 100_000
            };

            decimal expectedEarningsPerPaycheck = (getEmployeeDto.Salary / PaycheckConstants.PaychecksPerYear).RoundToNearestHundreths();

            decimal result = _paycheckCalculator.CalculateEarningsPerPaycheck(getEmployeeDto);

            Assert.Equal(expectedEarningsPerPaycheck, result);
        }

        // Even though the calculate methods dont expect null values (which is why they use `GetEmployeeDto`, rather than `GetEmployeeDto?`,
        // providing unit tests that pass null into them instead ensures the appropriate exception is thrown.
        [Fact]
        public void CalculateEarningsPerPaycheck_ForNullEmployee_ShouldReturnCorrectEarnings()
        {
            GetEmployeeDto getEmployeeDto = null;
            Assert.Throws<ArgumentNullException>(() => _paycheckCalculator.CalculateEarningsPerPaycheck(getEmployeeDto));
        }

        [Fact]
        public void CalculateBaseBenefitsDeduction_ShouldReturnCorrectDeduction()
        {
            decimal expectedCostPerPaycheck = PaycheckConstants.BaseBenefitsCostPerYear / PaycheckConstants.PaychecksPerYear;
            GetPaycheckDeductionDto expectedDto = new()
            {
                Label = PaycheckConstants.BaseBenefitsCostLabel,
                Amount = expectedCostPerPaycheck.RoundToNearestHundreths()
            };

            GetPaycheckDeductionDto result = _paycheckCalculator.CalculateBaseBenefitsDeduction();

            Assert.Equal(expectedDto.Label, result.Label);
            Assert.Equal(expectedDto.Amount, result.Amount);
        }

        [Fact]
        public void CalculateDependentsCountDeduction_ForEmployeeWithDependents_ShouldReturnCorrectDeduction()
        {
            GetEmployeeDto getEmployeeDto = new()
            {
                Dependents = new List<GetDependentDto>
                {
                    new() 
                    { 
                        DateOfBirth = new DateTime(1980, 1, 1) 
                    },
                    new() 
                    { 
                        DateOfBirth = new DateTime(1995, 1, 1) 
                    }
                }
            };

            decimal expectedCostPerPaycheck = getEmployeeDto.Dependents.Count * PaycheckConstants.DependentsBenefitsCostPerYear / PaycheckConstants.PaychecksPerYear;
            GetPaycheckDeductionDto expectedDto = new()
            {
                Label = PaycheckConstants.DependentsBenefitsCostLabel,
                Amount = expectedCostPerPaycheck.RoundToNearestHundreths()
            };

            GetPaycheckDeductionDto result = _paycheckCalculator.CalculateDependentsCountDeduction(getEmployeeDto);

            Assert.Equal(expectedDto.Label, result.Label);
            Assert.Equal(expectedDto.Amount, result.Amount);
        }

        [Fact]
        public void CalculateDependentsCountDeduction_ForEmployeeWithNoDependents_ShouldReturnCorrectDeduction()
        {
            GetEmployeeDto getEmployeeDto = new()
            {
                Dependents = new List<GetDependentDto>()
            };

            decimal expectedCostPerPaycheck = 0;
            GetPaycheckDeductionDto expectedDto = new()
            {
                Label = PaycheckConstants.DependentsBenefitsCostLabel,
                Amount = expectedCostPerPaycheck.RoundToNearestHundreths()
            };

            GetPaycheckDeductionDto result = _paycheckCalculator.CalculateDependentsCountDeduction(getEmployeeDto);

            Assert.Equal(expectedDto.Label, result.Label);
            Assert.Equal(expectedDto.Amount, result.Amount);
        }

        [Fact]
        public void CalculateDependentsCountDeduction_ForNullEmployee_ShouldThrowArgumentNullException()
        {
            GetEmployeeDto getEmployeeDto = null;

            Assert.Throws<ArgumentNullException>(() => _paycheckCalculator.CalculateDependentsCountDeduction(getEmployeeDto));
        }

        [Fact]
        public void CalculateDependentsOverAgeThresholdCountDeduction_ForEmployeeWithDependentsOverAgeThreshold_ShouldReturnCorrectDeduction()
        {
            GetEmployeeDto getEmployeeDto = new()
            {
                Dependents = new List<GetDependentDto>
                {
                    new()
                    {
                        DateOfBirth = new DateTime(1920, 1, 1)
                    },
                    new()
                    {
                        DateOfBirth = new DateTime(1925, 1, 1)
                    },
                    new()
                    {
                        DateOfBirth = DateTime.UtcNow.AddYears(-PaycheckConstants.AgeThresholdForAdditionalCost + 1) // This dependent should not be factored into this calculation.
                    }
                }
            };

            int numberOfDependentsOverAgeThreshold = getEmployeeDto.Dependents
                .Count(c => AgeHelper.GetAge(c.DateOfBirth) > PaycheckConstants.AgeThresholdForAdditionalCost);

            decimal costOfDependentsOverAgeThresholdPerYear = numberOfDependentsOverAgeThreshold * PaycheckConstants.DependentsAboveAgeThresholdBenefitsCostPerYear;
            decimal expectedCostPerPaycheck = costOfDependentsOverAgeThresholdPerYear / PaycheckConstants.PaychecksPerYear;

            GetPaycheckDeductionDto expectedDto = new()
            {
                Label = PaycheckConstants.DependentsAboveAgeThresholdBenefitsCostLabel,
                Amount = expectedCostPerPaycheck.RoundToNearestHundreths()
            };

            GetPaycheckDeductionDto result = _paycheckCalculator.CalculateDependentsOverAgeThresholdCountDeduction(getEmployeeDto);

            Assert.Equal(getEmployeeDto.Dependents.Count - 1, numberOfDependentsOverAgeThreshold);
            Assert.Equal(expectedDto.Label, result.Label);
            Assert.Equal(expectedDto.Amount, result.Amount);
        }

        [Fact]
        public void CalculateDependentsOverAgeThresholdCountDeduction_ForEmployeeWithNoDependentsOverAgeThreshold_ShouldReturnCorrectDeduction()
        {
            GetEmployeeDto getEmployeeDto = new()
            {
                Dependents = new List<GetDependentDto>
                {
                    new()
                    {
                        DateOfBirth = DateTime.UtcNow.AddYears(-PaycheckConstants.AgeThresholdForAdditionalCost + 1) // This dependent should not be factored into this calculation.
                    }
                }
            };

            int numberOfDependentsOverAgeThreshold = getEmployeeDto.Dependents
                .Count(c => AgeHelper.GetAge(c.DateOfBirth) > PaycheckConstants.AgeThresholdForAdditionalCost);

            decimal expectedCostPerPaycheck = 0;

            GetPaycheckDeductionDto expectedDto = new()
            {
                Label = PaycheckConstants.DependentsAboveAgeThresholdBenefitsCostLabel,
                Amount = expectedCostPerPaycheck.RoundToNearestHundreths()
            };

            GetPaycheckDeductionDto result = _paycheckCalculator.CalculateDependentsOverAgeThresholdCountDeduction(getEmployeeDto);

            Assert.Equal(getEmployeeDto.Dependents.Count - 1, numberOfDependentsOverAgeThreshold);
            Assert.Equal(expectedDto.Label, result.Label);
            Assert.Equal(expectedDto.Amount, result.Amount);
        }

        [Fact]
        public void CalculateDependentsOverAgeThresholdCountDeduction_ForEmployeeWithNoDependents_ShouldReturnCorrectDeduction()
        {
            GetEmployeeDto getEmployeeDto = new()
            {
                Dependents = new List<GetDependentDto>()
            };
            
            decimal expectedCostPerPaycheck = 0;

            GetPaycheckDeductionDto expectedDto = new()
            {
                Label = PaycheckConstants.DependentsAboveAgeThresholdBenefitsCostLabel,
                Amount = expectedCostPerPaycheck.RoundToNearestHundreths()
            };

            GetPaycheckDeductionDto result = _paycheckCalculator.CalculateDependentsOverAgeThresholdCountDeduction(getEmployeeDto);

            Assert.Equal(expectedDto.Label, result.Label);
            Assert.Equal(expectedDto.Amount, result.Amount);
        }

        [Fact]
        public void CalculateDependentsOverAgeThresholdCountDeduction_ForNullEmployee_ShouldThrowArgumentNullException()
        {
            GetEmployeeDto getEmployeeDto = null;

            Assert.Throws<ArgumentNullException>(() => _paycheckCalculator.CalculateDependentsOverAgeThresholdCountDeduction(getEmployeeDto));
        }

        [Fact]
        public void CalculateHighSalaryDeduction_ForEmployeeOverSalaryThreshold_ShouldReturnCorrectDeduction()
        {
            GetEmployeeDto getEmployeeDto = new()
            {
                Salary = PaycheckConstants.SalaryThresholdForAdditionalBenefits + 1
            };

            decimal expectedCostPerPaycheck = getEmployeeDto.Salary * PaycheckConstants.HighSalaryBenefitsMultiplier / PaycheckConstants.PaychecksPerYear;

            GetPaycheckDeductionDto expectedDto = new()
            {
                Label = PaycheckConstants.HighSalaryBenefitsCostLabel,
                Amount = expectedCostPerPaycheck.RoundToNearestHundreths()
            };

            GetPaycheckDeductionDto result = _paycheckCalculator.CalculateHighSalaryDeduction(getEmployeeDto);

            Assert.Equal(expectedDto.Label, result.Label);
            Assert.Equal(expectedDto.Amount, result.Amount);
        }

        [Fact]
        public void CalculateHighSalaryDeduction_ForEmployeeUnderSalaryThreshold_ShouldReturnCorrectDeduction()
        {
            GetEmployeeDto getEmployeeDto = new()
            {
                Salary = PaycheckConstants.SalaryThresholdForAdditionalBenefits  - 1
            };

            decimal expectedCostPerPaycheck = 0;

            GetPaycheckDeductionDto expectedDto = new()
            {
                Label = PaycheckConstants.HighSalaryBenefitsCostLabel,
                Amount = expectedCostPerPaycheck.RoundToNearestHundreths()
            };

            GetPaycheckDeductionDto result = _paycheckCalculator.CalculateHighSalaryDeduction(getEmployeeDto);

            Assert.Equal(expectedDto.Label, result.Label);
            Assert.Equal(expectedDto.Amount, result.Amount);
        }

        [Fact]
        public void CalculateHighSalaryDeduction_ForEmployeeAtSalaryThreshold_ShouldReturnCorrectDeduction()
        {
            GetEmployeeDto getEmployeeDto = new()
            {
                Salary = PaycheckConstants.SalaryThresholdForAdditionalBenefits
            };

            decimal expectedCostPerPaycheck = 0;

            GetPaycheckDeductionDto expectedDto = new()
            {
                Label = PaycheckConstants.HighSalaryBenefitsCostLabel,
                Amount = expectedCostPerPaycheck.RoundToNearestHundreths()
            };

            GetPaycheckDeductionDto result = _paycheckCalculator.CalculateHighSalaryDeduction(getEmployeeDto);

            Assert.Equal(expectedDto.Label, result.Label);
            Assert.Equal(expectedDto.Amount, result.Amount);
        }

        [Fact]
        public void CalculateHighSalaryDeduction_ForNullEmployee_ShouldThrowArgumentNullException()
        {
            GetEmployeeDto getEmployeeDto = null;

            Assert.Throws<ArgumentNullException>(() => _paycheckCalculator.CalculateHighSalaryDeduction(getEmployeeDto));
        }
    }
}
