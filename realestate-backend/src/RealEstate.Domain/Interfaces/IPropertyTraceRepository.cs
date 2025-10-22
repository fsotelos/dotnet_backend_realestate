using System.Collections.Generic;
using System.Threading.Tasks;
using RealEstate.Domain.Entities;

namespace RealEstate.Domain.Interfaces
{
    public interface IPropertyTraceRepository
    {
        Task<PropertyTrace> GetByIdAsync(string id);
        Task<IEnumerable<PropertyTrace>> GetAllAsync();
        Task<IEnumerable<PropertyTrace>> GetByPropertyIdAsync(string propertyId);
        Task<PropertyTrace> AddAsync(PropertyTrace propertyTrace);
        Task UpdateAsync(PropertyTrace propertyTrace);
        Task DeleteAsync(string id);
        Task<bool> ExistsAsync(string id);
        Task<IEnumerable<PropertyTrace>> GetByPropertyIdOrderedByDateAsync(string propertyId);
    }
}