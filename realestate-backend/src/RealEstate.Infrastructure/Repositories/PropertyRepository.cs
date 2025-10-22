using MongoDB.Bson;
using MongoDB.Driver;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Exceptions;
using RealEstate.Domain.Interfaces;
using RealEstate.Infrastructure.Persistence;

namespace RealEstate.Infrastructure.Repositories
{
    public class PropertyRepository : IPropertyRepository
    {
        private readonly IMongoCollection<Property> _properties;
        private readonly MongoDbContext _context;

        public PropertyRepository(MongoDbContext context)
        {
            _properties = context.Properties;
            _context = context;
        }

        public async Task<Property> GetByIdAsync(string id)
        {
            var property = await _properties.Find(p => p.Id == id).FirstOrDefaultAsync();
            if (property == null)
                throw new NotFoundException("Property", id);

            return property;
        }

        public async Task<IEnumerable<Property>> GetAllAsync()
        {
            return await _properties.Find(_ => true).ToListAsync();
        }

        public async Task<(IEnumerable<PropertyWithImages> Properties, int TotalCount)> GetFilteredAsync(string? name, string? address, decimal? minPrice, decimal? maxPrice, int page, int pageSize)
        {
            var filters = new List<FilterDefinition<Property>>();
            var builder = Builders<Property>.Filter;

            // Use $text search for name and address
            if (!string.IsNullOrEmpty(name) || !string.IsNullOrEmpty(address))
            {
                var search = $"{name} {address}".Trim();
                filters.Add(builder.Text(search));
            }

            // Add price filters
            if (minPrice.HasValue)
                filters.Add(builder.Gte(p => p.Price, minPrice.Value));

            if (maxPrice.HasValue)
                filters.Add(builder.Lte(p => p.Price, maxPrice.Value));

            var filter = filters.Any() ? builder.And(filters) : builder.Empty;

            // Get total count for pagination
            var totalCount = (int)await _properties.CountDocumentsAsync(filter);

            // Calculate skip value
            var skip = (page - 1) * pageSize;

            // Optimized aggregation pipeline with pagination
            var pipeline = await _properties
                         .Aggregate()
                         .Match(filter)
                         .Lookup(
                             _context.PropertyImages,
                             p => p.Id,
                             i => i.IdProperty,
                             (PropertyWithImages x) => x.Images
                         )
                        .Project(p => new PropertyWithImages
                        {
                            Id = p.Id,
                            Name = p.Name,
                            Address = p.Address,
                            Price = p.Price,
                            CodeInternal = p.CodeInternal,
                            Year = p.Year,
                            IdOwner = p.IdOwner,
                            Images = p.Images
                        })
                         .Skip(skip)
                         .Limit(pageSize)
                         .ToListAsync();

            return (pipeline, totalCount);
        }

     
        public async Task<Property> AddAsync(Property property)
        {
            await _properties.InsertOneAsync(property);
            return property;
        }

        public async Task UpdateAsync(Property property)
        {
            var result = await _properties.ReplaceOneAsync(p => p.Id == property.Id, property);
            if (result.MatchedCount == 0)
                throw new NotFoundException("Property", property.Id);
        }

        public async Task DeleteAsync(string id)
        {
            var result = await _properties.DeleteOneAsync(p => p.Id == id);
            if (result.DeletedCount == 0)
                throw new NotFoundException("Property", id);
        }

        public async Task<bool> ExistsAsync(string id)
        {
            return await _properties.Find(p => p.Id == id).AnyAsync();
        }

        public async Task<IEnumerable<Property>> GetByOwnerIdAsync(string ownerId)
        {
            return await _properties.Find(p => p.IdOwner == ownerId).ToListAsync();
        }
        
        public async Task AddManyAsync(IEnumerable<Property> properties)
        {
            await _properties.InsertManyAsync(properties);
        }
    }

}