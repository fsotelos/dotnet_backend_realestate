using RealEstate.Domain.Entities;
using FluentAssertions;

namespace RealEstate.Domain.Tests;

public class PropertyTests
{
    [Test]
    public void Property_Should_Be_Created_With_Valid_Data()
    {
        // Arrange
        var name = "Beautiful House";
        var address = "123 Main St";
        var price = 250000m;
        var codeInternal = "PROP001";
        var year = 2020;
        var idOwner = Guid.NewGuid().ToString();

        // Act
        var property = new Property(name, address, price, codeInternal, year, idOwner);

        // Assert
        property.Name.Should().Be(name);
        property.Address.Should().Be(address);
        property.Price.Should().Be(price);
        property.CodeInternal.Should().Be(codeInternal.ToUpper());
        property.Year.Should().Be(year);
        property.IdOwner.Should().Be(idOwner);
        property.Id.Should().NotBeNullOrEmpty();
    }

    [TestCase("")]
    [TestCase(null)]
    public void Property_Should_Throw_Exception_When_Name_Is_Invalid(string? invalidName)
    {
        // Arrange
        var address = "123 Main St";
        var price = 250000m;
        var codeInternal = "PROP001";
        var year = 2020;
        var idOwner = Guid.NewGuid().ToString();

        // Act & Assert
        var action = () => new Property(invalidName, address, price, codeInternal, year, idOwner);
        action.Should().Throw<ArgumentException>().WithMessage("*Name cannot be empty*");
    }

    [Test]
    public void Property_Should_Throw_Exception_When_Price_Is_Zero()
    {
        // Arrange
        var name = "Beautiful House";
        var address = "123 Main St";
        var price = 0m;
        var codeInternal = "PROP001";
        var year = 2020;
        var idOwner = Guid.NewGuid().ToString();

        // Act & Assert
        var action = () => new Property(name, address, price, codeInternal, year, idOwner);
        action.Should().Throw<ArgumentException>().WithMessage("*Price must be greater than zero*");
    }

    [Test]
    public void Property_Should_Add_Image_Successfully()
    {
        // Arrange
        var property = new Property("House", "Address", 100000, "CODE", 2020, Guid.NewGuid().ToString());
        var imageFile = "https://example.com/image.jpg";

        // Act
        property.AddImage(imageFile);

        // Assert
        property.Images.Should().HaveCount(1);
        property.Images.First().File.Should().Be(imageFile);
        property.Images.First().Enabled.Should().BeTrue();
    }

    [Test]
    public void Property_Should_Calculate_Age_Correctly()
    {
        // Arrange
        var currentYear = DateTime.Now.Year;
        var property = new Property("House", "Address", 100000, "CODE", currentYear - 5, Guid.NewGuid().ToString());

        // Act
        var age = property.GetPropertyAge();

        // Assert
        age.Should().Be(5);
    }
}