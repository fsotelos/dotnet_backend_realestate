using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Mappings;
using RealEstate.Infrastructure.Persistence;
using RealEstate.Presentation.Middleware;
using Serilog;

namespace RealEstate.Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = CreateHostBuilder(args);
            var app = builder.Build();

            Configure(app, builder);

            app.Run();
        }

        private static WebApplicationBuilder CreateHostBuilder(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
                .WriteTo.Seq("http://localhost:5341")
                .CreateLogger();

            builder.Host.UseSerilog();

            
            builder.Services.AddControllers();

            
            builder.Services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            });
            builder.Services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            
            builder.Services.AddAutoMapper(config =>
            {
                config.AddProfile<MappingProfile>();
            });

           
            builder.Services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(RealEstate.Application.Queries.GetPropertiesQuery).Assembly));

            
            builder.Services.AddValidatorsFromAssembly(typeof(RealEstate.Application.Queries.GetPropertiesQuery).Assembly);

            
            builder.Services.AddSingleton<MongoDbContext>();

            
            builder.Services.AddScoped<
                RealEstate.Domain.Interfaces.IPropertyRepository,
                RealEstate.Infrastructure.Repositories.PropertyRepository>();

            
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            return builder;
        }

        private static void Configure(WebApplication app, WebApplicationBuilder builder)
        {
            app.UseHttpsRedirection();
            app.UseCors("AllowAll");
            
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseAuthorization();
            app.MapControllers();
        }
    }
}
