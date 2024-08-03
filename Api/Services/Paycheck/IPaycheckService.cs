using Api.Dtos.Employee;
using Api.Dtos.Paycheck;

namespace Api.Services.PaycheckService
{
    public interface IPaycheckService
    {
        GetPaycheckDto GetPaycheck(GetEmployeeDto employee);
    }
}
