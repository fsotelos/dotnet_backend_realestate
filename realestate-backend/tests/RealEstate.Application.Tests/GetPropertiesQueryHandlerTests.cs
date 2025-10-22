using AutoMapper;
using Moq;
using RealEstate.Application.DTOs;
using RealEstate.Application.Handlers;
using RealEstate.Application.Queries;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;
using FluentAssertions;

namespace RealEstate.Application.Tests;

public class GetPropertiesQueryHandlerTests
{
    private readonly Mock<IPropertyRepository> _propertyRepositoryMock;
    private readonly IMapper _mapper;
    private readonly GetPropertiesQueryHandler _handler;

    public GetPropertiesQueryHandlerTests()
    {
        _propertyRepositoryMock = new Mock<IPropertyRepository>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<PropertyWithImages, PropertyDto>();
        });
        _mapper = new Mapper(config);

        _handler = new GetPropertiesQueryHandler(_propertyRepositoryMock.Object, _mapper);
    }

    [Fact]
    public async Task Handle_Should_Return_Paginated_Result_With_Properties()
    {
        // Arrange
        var query = new GetPropertiesQuery
        {
            Name = "Test Property",
            MinPrice = 100000,
            MaxPrice = 500000,
            Page = 1,
            PageSize = 10
        };

        var properties = new List<PropertyWithImages>
        {
            new PropertyWithImages
            {
                Id = "1",
                Name = "Test Property 1",
                Address = "123 Test St",
                Price = 250000,
                CodeInternal = "TP001",
                Year = 2020,
                IdOwner = "owner1",
                Images = new List<PropertyImage>
                {
                    new PropertyImage("1", "https://example.com/image1.jpg")
                }
            }
        };

        _propertyRepositoryMock
            .Setup(x => x.GetFilteredAsync("Test Property", null, 100000, 500000, 1, 10))
            .ReturnsAsync((properties, 1));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Properties.Should().HaveCount(1);
        result.TotalCount.Should().Be(1);
        result.Page.Should().Be(1);
        result.PageSize.Should().Be(10);
        result.TotalPages.Should().Be(1);
        result.Properties.First().Name.Should().Be("Test Property 1");
    }

    [Fact]
    public async Task Handle_Should_Return_Empty_Result_When_No_Properties_Found()
    {
        // Arrange
        var query = new GetPropertiesQuery
        {
            Name = "Nonexistent Property",
            Page = 1,
            PageSize = 10
        };

        _propertyRepositoryMock
            .Setup(x => x.GetFilteredAsync("Nonexistent Property", null, null, null, 1, 10))
            .ReturnsAsync((new List<PropertyWithImages>(), 0));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Properties.Should().BeEmpty();
        result.TotalCount.Should().Be(0);
        result.TotalPages.Should().Be(0);
    }

    [Fact]
    public async Task Handle_Should_Use_Default_Pagination_Values()
    {
        // Arrange
        var query = new GetPropertiesQuery
        {
            Name = "Test Property"
        };

        _propertyRepositoryMock
            .Setup(x => x.GetFilteredAsync("Test Property", null, null, null, 1, 10))
            .ReturnsAsync((new List<PropertyWithImages>(), 0));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Page.Should().Be(1);
        result.PageSize.Should().Be(10);
    }
}