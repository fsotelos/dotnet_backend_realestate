using MongoDB.Driver;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Exceptions;
using RealEstate.Domain.Interfaces;
using RealEstate.Infrastructure.Persistence;

namespace RealEstate.Infrastructure.Repositories
{
    public class PropertyTraceRepository : IPropertyTraceRepository
    {
        private readonly IMongoCollection<PropertyTrace> _propertyTraces;

        public PropertyTraceRepository(MongoDbContext context)
        {
            _propertyTraces = context.PropertyTraces;
        }

        public async Task<PropertyTrace> GetByIdAsync(string id)
        {
            var propertyTrace = await _propertyTraces.Find(t => t.Id == id).FirstOrDefaultAsync();
            if (propertyTrace == null)
                throw new NotFoundException("PropertyTrace", id);

            return propertyTrace;
        }

        public async Task<IEnumerable<PropertyTrace>> GetAllAsync()
        {
            return await _propertyTraces.Find(_ => true).ToListAsync();
        }

        public async Task<IEnumerable<PropertyTrace>> GetByPropertyIdAsync(string propertyId)
        {
            return await _propertyTraces.Find(t => t.IdProperty == propertyId).ToListAsync();
        }

        public async Task<PropertyTrace> AddAsync(PropertyTrace propertyTrace)
        {
            await _propertyTraces.InsertOneAsync(propertyTrace);
            return propertyTrace;
        }

        public async Task UpdateAsync(PropertyTrace propertyTrace)
        {
            var result = await _propertyTraces.ReplaceOneAsync(t => t.Id == propertyTrace.Id, propertyTrace);
            if (result.MatchedCount == 0)
                throw new NotFoundException("PropertyTrace", propertyTrace.Id);
        }

        public async Task DeleteAsync(string id)
        {
            var result = await _propertyTraces.DeleteOneAsync(t => t.Id == id);
            if (result.DeletedCount == 0)
                throw new NotFoundException("PropertyTrace", id);
        }

        public async Task<bool> ExistsAsync(string id)
        {
            return await _propertyTraces.Find(t => t.Id == id).AnyAsync();
        }

        public async Task<IEnumerable<PropertyTrace>> GetByPropertyIdOrderedByDateAsync(string propertyId)
        {
            return await _propertyTraces
                .Find(t => t.IdProperty == propertyId)
                .SortByDescending(t => t.DateSale)
                .ToListAsync();
        }
        public async Task AddManyAsync(IEnumerable<PropertyTrace> propertyTraces)
        {
            await _propertyTraces.InsertManyAsync(propertyTraces);
        }
    }
}