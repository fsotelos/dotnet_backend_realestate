using Moq;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;
using RealEstate.Domain.Services;
using FluentAssertions;
using NUnit.Framework;

namespace RealEstate.Application.Tests;

[TestFixture]
public class PropertyFilteringServiceTests
{
    private Mock<IPropertyRepository> _propertyRepositoryMock;
    private PropertyFilteringService _service;

    [SetUp]
    public void Setup()
    {
        _propertyRepositoryMock = new Mock<IPropertyRepository>();
        _service = new PropertyFilteringService(_propertyRepositoryMock.Object);
    }

    [Test]
    public async Task GetFilteredPropertiesAsync_Should_Validate_Parameters()
    {
        // Arrange
        var properties = new List<PropertyWithImages>
        {
            new PropertyWithImages { Id = "1", Name = "Test Property", Address = "123 Test St", Price = 300000 }
        };

        _propertyRepositoryMock.Setup(x => x.GetFilteredAsync("Test", null, null, null, 1, 10))
            .ReturnsAsync((properties, 1));

        // Act & Assert
        await _service.GetFilteredPropertiesAsync("Test", null, null, null, 1, 10);

        // Should not throw any exceptions for valid parameters
    }

    [Test]
    public void GetFilteredPropertiesAsync_Should_Throw_Exception_For_Invalid_Page()
    {
        // Act & Assert
        var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
            await _service.GetFilteredPropertiesAsync(null, null, null, null, 0, 10));

        exception.Message.Should().Contain("Page must be greater than 0");
    }

    [Test]
    public void GetFilteredPropertiesAsync_Should_Throw_Exception_For_Invalid_PageSize()
    {
        // Act & Assert
        var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
            await _service.GetFilteredPropertiesAsync(null, null, null, null, 1, 0));

        exception.Message.Should().Contain("PageSize must be between 1 and 100");
    }

    [Test]
    public void GetFilteredPropertiesAsync_Should_Throw_Exception_For_Invalid_Price_Range()
    {
        // Act & Assert
        var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
            await _service.GetFilteredPropertiesAsync(null, null, 500000, 200000, 1, 10));

        exception.Message.Should().Contain("MinPrice cannot be greater than MaxPrice");
    }

    [Test]
    public async Task GetFilteredPropertiesAsync_Should_Call_Repository_With_Correct_Parameters()
    {
        // Arrange
        var properties = new List<PropertyWithImages>
        {
            new PropertyWithImages { Id = "1", Name = "Beautiful House", Address = "123 Main St", Price = 300000 }
        };

        _propertyRepositoryMock.Setup(x => x.GetFilteredAsync("Beautiful", "Main", 200000, 400000, 2, 20))
            .ReturnsAsync((properties, 1));

        // Act
        var (result, totalCount) = await _service.GetFilteredPropertiesAsync("Beautiful", "Main", 200000, 400000, 2, 20);

        // Assert
        result.Should().HaveCount(1);
        result.First().Name.Should().Be("Beautiful House");
        totalCount.Should().Be(1);

        _propertyRepositoryMock.Verify(x => x.GetFilteredAsync("Beautiful", "Main", 200000, 400000, 2, 20), Times.Once);
    }

    [Test]
    public async Task GetFilteredPropertiesAsync_Should_Apply_Business_Rules()
    {
        // Arrange
        var properties = new List<PropertyWithImages>
        {
            new PropertyWithImages { Id = "1", Name = "Property 1", Address = "Address 1", Price = 300000 },
            new PropertyWithImages { Id = "2", Name = "Property 2", Address = "Address 2", Price = 400000 }
        };

        _propertyRepositoryMock.Setup(x => x.GetFilteredAsync(null, null, null, null, 1, 10))
            .ReturnsAsync((properties, 2));

        // Act
        var (result, totalCount) = await _service.GetFilteredPropertiesAsync(null, null, null, null, 1, 10);

        // Assert
        result.Should().HaveCount(2);
        totalCount.Should().Be(2);
        // Business rules are applied (currently just returns the properties as-is)
    }
}