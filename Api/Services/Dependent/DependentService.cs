using Api.Dtos.Dependent;
using Api.Extensions.DependentExtensions;
using Api.Models;
using Api.Repositories.DependentRepository;

namespace Api.Services.DependentService
{
    public class DependentService : IDependentService
    {
        private readonly IDependentRepository _dependentRepository;
        
        public DependentService(IDependentRepository dependentRepository) 
        {
            _dependentRepository = dependentRepository;
        }

        public async Task<GetDependentDto?> GetDependent(int dependentId)
        {
            Dependent? dependent = await _dependentRepository.GetDependent(dependentId);
            if (dependent == null)
            {
                return null;
            }

            GetDependentDto getDependentDto = dependent.ToGetDependentDto();
            
            return getDependentDto;
        }

        public async Task<List<GetDependentDto>> GetAllDependents()
        {
            List<Dependent> dependents = await _dependentRepository.GetAllDependents();

            List<GetDependentDto> getDependentDtos = dependents.Select(s => s.ToGetDependentDto()).ToList();
            
            return getDependentDtos;
        }
    }
}
