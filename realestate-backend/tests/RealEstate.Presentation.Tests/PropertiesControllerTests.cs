using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using RealEstate.Application.DTOs;
using System.Net;
using System.Net.Http.Json;

namespace RealEstate.Presentation.Tests;

public class PropertiesControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public PropertiesControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
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
    }

    [Fact]
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
    }

    [Fact]
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
    }

    [Fact]
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

    [Fact]
    public async Task GetProperties_Should_Return_Json_Content_Type()
    {
        // Act
        var response = await _client.GetAsync("/api/v1.0/properties");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Be("application/json");
    }
}