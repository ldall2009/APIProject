using Api.Dtos.Paycheck;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace ApiTests.IntegrationTests
{
    public class PaycheckIntegrationTests : IntegrationTest
    {
        [Fact]
        public async Task WhenAskedForAnEmployeePaycheckWhereEmployeeHasNoDependents_ShouldReturnCorrectPaycheck()
        {
            var response = await HttpClient.GetAsync($"/api/v1/paychecks/employee/1");
            var paycheck = new GetPaycheckDto
            {
                EmployeeName = "LeBron James",
                TotalEarnings = 2900.81m,
                Deductions = new List<GetPaycheckDeductionDto>()
                {
                    new()
                    {
                        Label = "Base Benefits Cost",
                        Amount = 461.54m,
                    },
                }
            };
            await response.ShouldReturn(HttpStatusCode.OK, paycheck);
        }

        [Fact]
        public async Task WhenAskedForAnEmployeePaycheckWhereEmployeeHasDependents_ShouldReturnCorrectPaycheck()
        {
            var response = await HttpClient.GetAsync($"/api/v1/paychecks/employee/3");
            var paycheck = new GetPaycheckDto
            {
                EmployeeName = "Michael Jordan",
                TotalEarnings = 5508.12m,
                Deductions = new List<GetPaycheckDeductionDto>()
                {
                    new()
                    {
                        Label = "Base Benefits Cost",
                        Amount = 461.54m,
                    },
                    new()
                    {
                        Label = "Benefits Cost for Number of Dependents",
                        Amount = 276.92m,
                    },
                    new()
                    {
                        Label = "High Salary Benefits Cost",
                        Amount = 110.16m,
                    },
                }
            };
            await response.ShouldReturn(HttpStatusCode.OK, paycheck);
        }

        [Fact]
        public async Task WhenAskedForAPaycheckForANonexistentEmployee_ShouldReturn404()
        {
            var response = await HttpClient.GetAsync($"/api/v1/paychecks/employee/{int.MinValue}");
            await response.ShouldReturn(HttpStatusCode.NotFound);
        }
    }
}
