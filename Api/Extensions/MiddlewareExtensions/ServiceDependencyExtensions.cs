using Api.Services.DependentService;
using Api.Services.EmployeeService;
using Api.Services.Paycheck.Calculators;
using Api.Services.PaycheckService;

namespace Api.Extensions.MiddlewareExtensions
{
    public static class ServiceDependencyExtensions
    {
        public static IServiceCollection AddServiceDependencies(this IServiceCollection services)
        {
            // Technically given the current app complexities, these could just be Transient, but as services grow they could
            // potentially need to be called more than once in a HTTP request while needing to maintain some state data, so I will assign them to the Scoped lifetime.
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IDependentService, DependentService>();
            services.AddScoped<IPaycheckService, PaycheckService>();

            // I forecast the paycheck calculator will remain stateless & lightweight, which is why I have assigned this one to Transient.
            services.AddTransient<IPaycheckCalculator, PaycheckCalculator>();
            
            return services;
        }
    }
}
