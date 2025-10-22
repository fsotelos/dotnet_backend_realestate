using System.Collections.Generic;
using System.Threading.Tasks;
using RealEstate.Domain.Entities;

namespace RealEstate.Domain.Interfaces
{
    public interface IPropertyImageRepository
    {
        Task<PropertyImage> GetByIdAsync(string id);
        Task<IEnumerable<PropertyImage>> GetAllAsync();
        Task<IEnumerable<PropertyImage>> GetByPropertyIdAsync(string propertyId);
        Task<PropertyImage> AddAsync(PropertyImage propertyImage);
        Task UpdateAsync(PropertyImage propertyImage);
        Task DeleteAsync(string id);
        Task<bool> ExistsAsync(string id);
        Task<IEnumerable<PropertyImage>> GetEnabledByPropertyIdAsync(string propertyId);
    }
}