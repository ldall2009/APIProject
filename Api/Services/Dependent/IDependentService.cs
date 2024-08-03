using Api.Dtos.Dependent;

namespace Api.Services.DependentService
{
    public interface IDependentService
    {
        Task<GetDependentDto?> GetDependent(int dependentId);
        Task<List<GetDependentDto>> GetAllDependents();
    }
}
