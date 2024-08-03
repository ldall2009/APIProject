using Api.Repositories.DependentRepository;
using Api.Repositories.EmployeeRepository;

namespace Api.Extensions.MiddlewareExtensions
{
    public static class RepositoryDependencyExtensions
    {
        public static IServiceCollection AddRepositoryDependencies(this IServiceCollection services)
        {
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IDependentRepository, DependentRepository>();

            return services;
        }
    }
}
