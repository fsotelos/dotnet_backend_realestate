using MongoDB.Driver;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Exceptions;
using RealEstate.Domain.Interfaces;
using RealEstate.Infrastructure.Persistence;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealEstate.Infrastructure.Repositories
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly IMongoCollection<Owner> _owners;

        public OwnerRepository(MongoDbContext context)
        {
            _owners = context.Owners;
        }

        public async Task<Owner> GetByIdAsync(string id)
        {
            var owner = await _owners.Find(o => o.Id == id).FirstOrDefaultAsync();
            if (owner == null)
                throw new NotFoundException("Owner", id);

            return owner;
        }

        public async Task<IEnumerable<Owner>> GetAllAsync()
        {
            return await _owners.Find(_ => true).ToListAsync();
        }

        public async Task<Owner> AddAsync(Owner owner)
        {
            await _owners.InsertOneAsync(owner);
            return owner;
        }

        public async Task UpdateAsync(Owner owner)
        {
            var result = await _owners.ReplaceOneAsync(o => o.Id == owner.Id, owner);
            if (result.MatchedCount == 0)
                throw new NotFoundException("Owner", owner.Id);
        }

        public async Task DeleteAsync(string id)
        {
            var result = await _owners.DeleteOneAsync(o => o.Id == id);
            if (result.DeletedCount == 0)
                throw new NotFoundException("Owner", id);
        }

        public async Task<bool> ExistsAsync(string id)
        {
            return await _owners.Find(o => o.Id == id).AnyAsync();
        }

        // Bulk operations for data seeding
        public async Task AddManyAsync(IEnumerable<Owner> owners)
        {
            await _owners.InsertManyAsync(owners);
        }
    }
}