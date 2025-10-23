# RealEstate Backend API

A RESTful API built with **.NET 9** and **C#** for managing property data in a MongoDB database. This backend serves as part of a full-stack technical test for a Senior Frontend Developer position at a large real estate company, providing endpoints to fetch, filter, and manage property information for integration with a ReactJS or Next.js frontend.

## 🚀 Features

- Fetch properties from MongoDB with pagination support.
- Filter properties by name, address, and price range.
- Retrieve a single property by ID.
- Support for property DTO fields:
  - `IdOwner`
  - `Name`
  - `AddressProperty`
  - `PriceProperty`
  - `ImageUrl` (single image).

## 🏗️ Architecture

This project follows **Clean Architecture** principles, separating concerns into distinct layers for maintainability and testability:

- **Domain Layer**: Contains core business entities, interfaces, services, and domain logic (e.g., `Property`, `IPropertyRepository`, `PropertyFilteringService`).
- **Application Layer**: Handles use cases, queries, commands, DTOs, and mappings (e.g., `GetPropertiesQuery`, `GetPropertiesQueryHandler`, `PropertyDto`).
- **Infrastructure Layer**: Manages data access, external services, and persistence (e.g., `PropertyRepository`, `MongoDbContext`).
- **Presentation Layer**: Exposes the API via controllers, middleware, and configuration (e.g., `PropertiesController`, `ExceptionHandlingMiddleware`).

The architecture leverages dependency injection for loose coupling, configuration files for environment-specific settings, and DTOs for data transfer between layers.

## 🛠️ Technologies Used

- **.NET 9** - Framework for building the API.
- **C#** - Primary programming language.
- **MongoDB** - NoSQL database for property data storage.
- **NUnit** - Framework for unit testing.

## 📡 API Endpoints

| Method | Endpoint                  | Description                       |
|--------|---------------------------|-----------------------------------|
| GET    | /api/properties           | Get all properties with optional filters (name, address, minPrice, maxPrice) and pagination (page, pageSize) |
| GET    | /api/properties/{id}      | Get property by ID                |

## 🚀 Installation and Setup

### Prerequisites
- .NET 9 SDK (download from [Microsoft](https://dotnet.microsoft.com/download/dotnet/9.0))
- Docker (for running MongoDB in a container)
- MongoDB (alternatively, run locally via Docker as described below)

### Steps
1. Clone the repository:
   ```bash
   git clone https://github.com/your-repo/realestate-backend.git
   cd realestate-backend
   ```

2. Start MongoDB using Docker:
   ```bash
   docker run -d -p 27017:27017 --name mongodb mongo:latest
   ```
   This runs MongoDB in a Docker container on port 27017.

3. Configure MongoDB connection string in `src/RealEstate.Presentation/appsettings.json` and `src/RealEstate.Console/appsettings.json`:
   ```json
   {
     "MongoSettings": {
       "ConnectionString": "mongodb://localhost:27017",
       "DatabaseName": "RealEstateDb"
     }
   }
   ```

4. Restore dependencies and build the project:
   ```bash
   dotnet restore
   dotnet build
   ```

5. (Optional) Seed the database with sample data using the Console app:
   ```bash
   dotnet run --project src/RealEstate.Console -- 1000 5000 15000 10000 1000
   ```
   This seeds 1000 owners, 5000 properties, 15000 property images, 10000 property traces, with a batch size of 1000. Adjust arguments as needed (defaults: 1000 owners, 5000 properties, etc.).

6. Run the API project:
   ```bash
   dotnet run --project src/RealEstate.Presentation
   ```

The API will be available at `https://localhost:5001` (or `http://localhost:5000` for HTTP).

## 🧪 Testing

Unit tests are written using NUnit. To run the tests:

```bash
dotnet test
```

For coverage reports, use the provided `coverage.runsettings` files in the test projects.

## ⚡ Performance and Best Practices

- **Clean Architecture**: Ensures separation of concerns, making the codebase modular and easy to maintain.
- **Dependency Injection**: Promotes loose coupling and facilitates testing.
- **Error Handling**: Implemented via middleware (`ExceptionHandlingMiddleware`) for consistent API responses.
- **Query Optimization**: Filtering and pagination are optimized for handling large datasets efficiently.
- **Validation**: Domain entities include business rules and validation to ensure data integrity.

## 📸 Screenshots / Demo


[▶️ Watch Demo Video]
https://youtu.be/3lNAKKzeXmQ

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👤 Author

**Felipe Sotelo**  
**Email:** felipe.sotelo@live.com
