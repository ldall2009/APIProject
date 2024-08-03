using Api.Models;

namespace Api.Repositories.DependentRepository
{
    public interface IDependentRepository
    {
        Task<Dependent?> GetDependent(int dependentId);
        Task<List<Dependent>> GetAllDependents();
    }
}
