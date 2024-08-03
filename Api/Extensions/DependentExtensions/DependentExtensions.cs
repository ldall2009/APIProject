using Api.Dtos.Dependent;
using Api.Models;

namespace Api.Extensions.DependentExtensions
{
    public static class DependentExtensions
    {
        public static GetDependentDto ToGetDependentDto(this Dependent dependent)
        {
            return new()
            {
                Id = dependent.Id,
                FirstName = dependent.FirstName,
                LastName = dependent.LastName,
                DateOfBirth = dependent.DateOfBirth,
                Relationship = dependent.Relationship,
            };
        }
    }
}
