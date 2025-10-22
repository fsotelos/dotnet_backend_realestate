using MongoDB.Driver;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Exceptions;
using RealEstate.Domain.Interfaces;
using RealEstate.Infrastructure.Persistence;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealEstate.Infrastructure.Repositories
{
    public class PropertyImageRepository : IPropertyImageRepository
    {
        private readonly IMongoCollection<PropertyImage> _propertyImages;

        public PropertyImageRepository(MongoDbContext context)
        {
            _propertyImages = context.PropertyImages;
        }

        public async Task<PropertyImage> GetByIdAsync(string id)
        {
            var propertyImage = await _propertyImages.Find(i => i.Id == id).FirstOrDefaultAsync();
            if (propertyImage == null)
                throw new NotFoundException("PropertyImage", id);

            return propertyImage;
        }

        public async Task<IEnumerable<PropertyImage>> GetAllAsync()
        {
            return await _propertyImages.Find(_ => true).ToListAsync();
        }

        public async Task<IEnumerable<PropertyImage>> GetByPropertyIdAsync(string propertyId)
        {
            return await _propertyImages.Find(i => i.IdProperty == propertyId).ToListAsync();
        }

        public async Task<PropertyImage> AddAsync(PropertyImage propertyImage)
        {
            await _propertyImages.InsertOneAsync(propertyImage);
            return propertyImage;
        }

        public async Task UpdateAsync(PropertyImage propertyImage)
        {
            var result = await _propertyImages.ReplaceOneAsync(i => i.Id == propertyImage.Id, propertyImage);
            if (result.MatchedCount == 0)
                throw new NotFoundException("PropertyImage", propertyImage.Id);
        }

        public async Task DeleteAsync(string id)
        {
            var result = await _propertyImages.DeleteOneAsync(i => i.Id == id);
            if (result.DeletedCount == 0)
                throw new NotFoundException("PropertyImage", id);
        }

        public async Task<bool> ExistsAsync(string id)
        {
            return await _propertyImages.Find(i => i.Id == id).AnyAsync();
        }

        public async Task<IEnumerable<PropertyImage>> GetEnabledByPropertyIdAsync(string propertyId)
        {
            return await _propertyImages.Find(i => i.IdProperty == propertyId && i.Enabled).ToListAsync();
        }

        // Bulk operations for data seeding
        public async Task AddManyAsync(IEnumerable<PropertyImage> propertyImages)
        {
            await _propertyImages.InsertManyAsync(propertyImages);
        }
    }
}