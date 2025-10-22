using System.Collections.Generic;
using System.Threading.Tasks;
using RealEstate.Domain.Entities;

namespace RealEstate.Domain.Interfaces
{
    public interface IPropertyRepository
    {
        Task<Property> GetByIdAsync(string id);
        Task<IEnumerable<Property>> GetAllAsync();
        Task<(IEnumerable<PropertyWithImages> Properties, int TotalCount)> GetFilteredAsync(string? name, string? address, decimal? minPrice, decimal? maxPrice, int page, int pageSize);
        Task<Property> AddAsync(Property property);
        Task UpdateAsync(Property property);
        Task DeleteAsync(string id);
        Task<bool> ExistsAsync(string id);
        Task<IEnumerable<Property>> GetByOwnerIdAsync(string ownerId);
    }
}