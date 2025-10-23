using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;

namespace RealEstate.Domain.Services
{
    public class PropertyFilteringService : IPropertyFilteringService
    {
        private readonly IPropertyRepository _propertyRepository;

        public PropertyFilteringService(IPropertyRepository propertyRepository)
        {
            _propertyRepository = propertyRepository;
        }

        public async Task<(IEnumerable<PropertyWithImages> Properties, int TotalCount)> GetFilteredPropertiesAsync(
            string? name,
            string? address,
            decimal? minPrice,
            decimal? maxPrice,
            int page,
            int pageSize)
        {
            // Validate input parameters
            ValidateFilteringParameters(name, address, minPrice, maxPrice, page, pageSize);

            // Apply business rules for filtering
            var (properties, totalCount) = await _propertyRepository.GetFilteredAsync(
                name, address, minPrice, maxPrice, page, pageSize);

            // Apply additional business logic if needed
            var filteredProperties = ApplyBusinessRules(properties, name, address, minPrice, maxPrice);

            return (filteredProperties, totalCount);
        }

        private void ValidateFilteringParameters(string? name, string? address, decimal? minPrice, decimal? maxPrice, int page, int pageSize)
        {
            if (page < 1)
                throw new ArgumentException("Page must be greater than 0", nameof(page));

            if (pageSize < 1 || pageSize > 100)
                throw new ArgumentException("PageSize must be between 1 and 100", nameof(pageSize));

            if (minPrice.HasValue && minPrice.Value < 0)
                throw new ArgumentException("MinPrice cannot be negative", nameof(minPrice));

            if (maxPrice.HasValue && maxPrice.Value < 0)
                throw new ArgumentException("MaxPrice cannot be negative", nameof(maxPrice));

            if (minPrice.HasValue && maxPrice.HasValue && minPrice.Value > maxPrice.Value)
                throw new ArgumentException("MinPrice cannot be greater than MaxPrice");

            if (!string.IsNullOrEmpty(name) && name.Length > 100)
                throw new ArgumentException("Name filter cannot exceed 100 characters", nameof(name));

            if (!string.IsNullOrEmpty(address) && address.Length > 200)
                throw new ArgumentException("Address filter cannot exceed 200 characters", nameof(address));
        }

        private IEnumerable<PropertyWithImages> ApplyBusinessRules(
            IEnumerable<PropertyWithImages> properties,
            string? name,
            string? address,
            decimal? minPrice,
            decimal? maxPrice)
        {
            // Apply any additional business rules here
            // For example: exclude properties that are not active, apply custom scoring, etc.

            // Currently, the repository handles the basic filtering, but we can add more complex logic here
            // such as filtering by property status, applying custom search algorithms, etc.

            return properties;
        }
    }
}