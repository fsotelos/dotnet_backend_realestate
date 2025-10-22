using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;
using RealEstate.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstate.Infrastructure.Services
{
    public class DataSeeder
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly IPropertyRepository _propertyRepository;
        private readonly IPropertyImageRepository _propertyImageRepository;
        private readonly IPropertyTraceRepository _propertyTraceRepository;
        private readonly DataGenerator _dataGenerator;
        private readonly ILogger<DataSeeder> _logger;
        private readonly MongoDbContext _mongoDbContext;

        public DataSeeder(
            IOwnerRepository ownerRepository,
            IPropertyRepository propertyRepository,
            IPropertyImageRepository propertyImageRepository,
            IPropertyTraceRepository propertyTraceRepository,
            DataGenerator dataGenerator,
            ILogger<DataSeeder> logger,
            MongoDbContext mongoDbContext)
        {
            _ownerRepository = ownerRepository;
            _propertyRepository = propertyRepository;
            _propertyImageRepository = propertyImageRepository;
            _propertyTraceRepository = propertyTraceRepository;
            _dataGenerator = dataGenerator;
            _logger = logger;
            _mongoDbContext = mongoDbContext;
        }

        public async Task SeedDataAsync(DataSeedingOptions options)
        {
            _logger.LogInformation("Starting data seeding with options: {@Options}", options);

            try
            {
                // Generate and seed owners first
                var owners = _dataGenerator.GenerateOwners(options.OwnerCount).ToList();
                await SeedOwnersAsync(owners, options.BatchSize);

                var ownerIds = owners.Select(o => o.Id).ToList();

                // Generate and seed properties
                var properties = _dataGenerator.GenerateProperties(options.PropertyCount, ownerIds).ToList();
                await SeedPropertiesAsync(properties, options.BatchSize);

                // Create text index on Properties collection
                await CreateTextIndexAsync();

                var propertyIds = properties.Select(p => p.Id).ToList();

                // Generate and seed property images
                var propertyImages = _dataGenerator.GeneratePropertyImages(options.PropertyImageCount, propertyIds).ToList();
                await SeedPropertyImagesAsync(propertyImages, options.BatchSize);

                // Generate and seed property traces
                var propertyTraces = _dataGenerator.GeneratePropertyTraces(options.PropertyTraceCount, propertyIds).ToList();
                await SeedPropertyTracesAsync(propertyTraces, options.BatchSize);

                _logger.LogInformation("Data seeding completed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during data seeding");
                throw;
            }
        }

        private async Task SeedOwnersAsync(List<Owner> owners, int batchSize)
        {
            await ProcessInBatchesAsync(owners, batchSize, async batch =>
            {
                await ((dynamic)_ownerRepository).AddManyAsync(batch);
                _logger.LogInformation("Seeded {Count} owners", batch.Count);
            });
        }

        private async Task SeedPropertiesAsync(List<Property> properties, int batchSize)
        {
            await ProcessInBatchesAsync(properties, batchSize, async batch =>
            {
                await ((dynamic)_propertyRepository).AddManyAsync(batch);
                _logger.LogInformation("Seeded {Count} properties", batch.Count);
            });
        }

        private async Task SeedPropertyImagesAsync(List<PropertyImage> propertyImages, int batchSize)
        {
            await ProcessInBatchesAsync(propertyImages, batchSize, async batch =>
            {
                await ((dynamic)_propertyImageRepository).AddManyAsync(batch);
                _logger.LogInformation("Seeded {Count} property images", batch.Count);
            });
        }

        private async Task SeedPropertyTracesAsync(List<PropertyTrace> propertyTraces, int batchSize)
        {
            await ProcessInBatchesAsync(propertyTraces, batchSize, async batch =>
            {
                await ((dynamic)_propertyTraceRepository).AddManyAsync(batch);
                _logger.LogInformation("Seeded {Count} property traces", batch.Count);
            });
        }

        private async Task ProcessInBatchesAsync<T>(List<T> items, int batchSize, Func<List<T>, Task> processBatch)
        {
            for (int i = 0; i < items.Count; i += batchSize)
            {
                var batch = items.Skip(i).Take(batchSize).ToList();
                await processBatch(batch);
            }
        }

        private async Task CreateTextIndexAsync()
        {
            try
            {
                var indexKeys = Builders<Property>.IndexKeys.Text(p => p.Name).Text(p => p.Address);
                var indexModel = new CreateIndexModel<Property>(indexKeys, new CreateIndexOptions { Name = "TextIndex_Name_Address" });
                await _mongoDbContext.Properties.Indexes.CreateOneAsync(indexModel);
                _logger.LogInformation("Created text index on Properties collection for Name and Address fields");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to create text index, it might already exist");
            }
        }
    }

    public class DataSeedingOptions
    {
        public int OwnerCount { get; set; } = 1000;
        public int PropertyCount { get; set; } = 5000;
        public int PropertyImageCount { get; set; } = 15000;
        public int PropertyTraceCount { get; set; } = 10000;
        public int BatchSize { get; set; } = 1000;
    }
}