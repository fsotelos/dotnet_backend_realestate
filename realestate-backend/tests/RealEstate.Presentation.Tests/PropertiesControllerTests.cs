using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using RealEstate.Application.DTOs;
using System.Net;
using System.Net.Http.Json;

namespace RealEstate.Presentation.Tests;

[TestFixture]
public class PropertiesControllerTests
{
    private WebApplicationFactory<Program> _factory;
    private HttpClient _client;

    [SetUp]
    public void Setup()
    {
        _factory = new WebApplicationFactory<Program>();
        _client = _factory.CreateClient();
    }

    [TearDown]
    public void TearDown()
    {
        _client?.Dispose();
        _factory?.Dispose();
    }

    [Test]
    public async Task GetProperties_Should_Return_Ok_With_Valid_Query()
    {
        // Arrange
        var queryParams = "?name=Test&page=1&pageSize=10";

        // Act
        var response = await _client.GetAsync($"/api/v1.0/properties{queryParams}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<PaginatedPropertyDto>();
        result.Should().NotBeNull();
        result!.Page.Should().Be(1);
        result.PageSize.Should().Be(10);
        // Verify that all returned properties match the name filter
        foreach (var property in result.Properties)
        {
            property.Name.Should().Contain("Test", because: "Name filter should match");
        }
    }

    [Test]
    public async Task GetProperties_Should_Return_Ok_With_Price_Filters()
    {
        // Arrange
        var queryParams = "?minPrice=100000&maxPrice=500000&page=1&pageSize=10";

        // Act
        var response = await _client.GetAsync($"/api/v1.0/properties{queryParams}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<PaginatedPropertyDto>();
        result.Should().NotBeNull();
        result!.Properties.Should().NotBeNull();
        // Verify that all returned properties are within the price range
        foreach (var property in result.Properties)
        {
            property.Price.Should().BeGreaterOrEqualTo(100000);
            property.Price.Should().BeLessOrEqualTo(500000);
        }
    }

    [Test]
    public async Task GetProperties_Should_Return_Ok_With_Text_Search()
    {
        // Arrange
        var queryParams = "?name=House&address=Street&page=1&pageSize=10";

        // Act
        var response = await _client.GetAsync($"/api/v1.0/properties{queryParams}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<PaginatedPropertyDto>();
        result.Should().NotBeNull();
        // Verify that all returned properties match the search criteria
        foreach (var property in result.Properties)
        {
            property.Name.Should().Contain("House", because: "Name filter should match");
            property.Address.Should().Contain("Street", because: "Address filter should match");
        }
    }

    [Test]
    public async Task GetProperties_Should_Use_Default_Pagination_When_Not_Specified()
    {
        // Act
        var response = await _client.GetAsync("/api/v1.0/properties");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<PaginatedPropertyDto>();
        result.Should().NotBeNull();
        result!.Page.Should().Be(1);
        result.PageSize.Should().Be(10);
    }

    [Test]
    public async Task GetProperties_Should_Return_Json_Content_Type()
    {
        // Act
        var response = await _client.GetAsync("/api/v1.0/properties");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Be("application/json");
    }
}