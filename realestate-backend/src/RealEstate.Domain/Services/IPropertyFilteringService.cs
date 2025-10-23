using RealEstate.Domain.Entities;

namespace RealEstate.Domain.Services
{
    public interface IPropertyFilteringService
    {
        Task<(IEnumerable<PropertyWithImages> Properties, int TotalCount)> GetFilteredPropertiesAsync(
            string? name,
            string? address,
            decimal? minPrice,
            decimal? maxPrice,
            int page,
            int pageSize);
    }
}