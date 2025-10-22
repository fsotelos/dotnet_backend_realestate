using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RealEstate.Domain.Interfaces;
using RealEstate.Infrastructure.Persistence;
using RealEstate.Infrastructure.Repositories;
using RealEstate.Infrastructure.Services;

namespace RealEstate.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            System.Console.WriteLine("Real Estate Data Seeder Console Application");
            System.Console.WriteLine("==========================================");

            // Build configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Setup dependency injection
            var services = new ServiceCollection();

            // Add logging
            services.AddLogging(configure => configure.AddConsole());

            // Add configuration
            services.AddSingleton<IConfiguration>(configuration);

            // Add MongoDB context
            services.AddSingleton<MongoDbContext>();

            // Register repositories
            services.AddScoped<IOwnerRepository, OwnerRepository>();
            services.AddScoped<IPropertyRepository, PropertyRepository>();
            services.AddScoped<IPropertyImageRepository, PropertyImageRepository>();
            services.AddScoped<IPropertyTraceRepository, PropertyTraceRepository>();

            // Register services
            services.AddScoped<DataGenerator>();
            services.AddScoped<DataSeeder>();

            var serviceProvider = services.BuildServiceProvider();

            try
            {
                var dataSeeder = serviceProvider.GetRequiredService<DataSeeder>();
                var mongoDbContext = serviceProvider.GetRequiredService<MongoDbContext>();
                var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

                // Configure seeding options
                var options = new DataSeedingOptions
                {
                    OwnerCount = GetIntFromArgs(args, 0, 1000),
                    PropertyCount = GetIntFromArgs(args, 1, 5000),
                    PropertyImageCount = GetIntFromArgs(args, 2, 15000),
                    PropertyTraceCount = GetIntFromArgs(args, 3, 10000),
                    BatchSize = GetIntFromArgs(args, 4, 1000)
                };

                System.Console.WriteLine($"Seeding configuration:");
                System.Console.WriteLine($"  Owners: {options.OwnerCount}");
                System.Console.WriteLine($"  Properties: {options.PropertyCount}");
                System.Console.WriteLine($"  Property Images: {options.PropertyImageCount}");
                System.Console.WriteLine($"  Property Traces: {options.PropertyTraceCount}");
                System.Console.WriteLine($"  Batch Size: {options.BatchSize}");
                System.Console.WriteLine();

                System.Console.WriteLine("Dropping existing database...");
                await mongoDbContext.DropDatabaseAsync();
                System.Console.WriteLine("Database dropped successfully.");

                System.Console.WriteLine("Starting data seeding...");
                var startTime = DateTime.Now;

                await dataSeeder.SeedDataAsync(options);

                var duration = DateTime.Now - startTime;
                System.Console.WriteLine($"Data seeding completed successfully in {duration.TotalSeconds:F2} seconds!");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error during data seeding: {ex.Message}");
                Environment.Exit(1);
            }
        }

        private static int GetIntFromArgs(string[] args, int index, int defaultValue)
        {
            if (args.Length > index && int.TryParse(args[index], out var value))
            {
                return value;
            }
            return defaultValue;
        }
    }
}