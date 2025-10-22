using System.Collections.Generic;
using System.Threading.Tasks;
using RealEstate.Domain.Entities;

namespace RealEstate.Domain.Interfaces
{
    public interface IOwnerRepository
    {
        Task<Owner> GetByIdAsync(string id);
        Task<IEnumerable<Owner>> GetAllAsync();
        Task<Owner> AddAsync(Owner owner);
        Task UpdateAsync(Owner owner);
        Task DeleteAsync(string id);
        Task<bool> ExistsAsync(string id);
    }
}