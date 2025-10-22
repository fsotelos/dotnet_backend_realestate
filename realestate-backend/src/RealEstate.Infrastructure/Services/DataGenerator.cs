using Bogus;
using RealEstate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RealEstate.Infrastructure.Services
{
    public class DataGenerator
    {
        private readonly Faker<Owner> _ownerFaker;
        private readonly Faker<Property> _propertyFaker;
        private readonly Faker<PropertyImage> _propertyImageFaker;
        private readonly Faker<PropertyTrace> _propertyTraceFaker;

        public DataGenerator()
        {
            // Configure Owner faker with diverse, realistic data
            _ownerFaker = new Faker<Owner>()
                .CustomInstantiator(f => new Owner(
                    f.Name.FullName(),
                    f.Address.FullAddress(),
                    f.Internet.Avatar(),
                    f.Date.Past(80, DateTime.Now.AddYears(-18))));

            // Configure Property faker
            _propertyFaker = new Faker<Property>()
                .RuleFor(p => p.Name, f => f.Company.CompanyName() + " Property")
                .RuleFor(p => p.Address, f => f.Address.FullAddress())
                .RuleFor(p => p.Price, f => f.Random.Decimal(50000, 5000000))
                .RuleFor(p => p.CodeInternal, f => f.Random.AlphaNumeric(10).ToUpper())
                .RuleFor(p => p.Year, f => f.Random.Int(1900, DateTime.Now.Year));

            // Configure PropertyImage faker
            _propertyImageFaker = new Faker<PropertyImage>()
                .RuleFor(i => i.File, f => f.Image.PicsumUrl())
                .RuleFor(i => i.Enabled, f => f.Random.Bool(0.8f)); // 80% enabled

            // Configure PropertyTrace faker
            _propertyTraceFaker = new Faker<PropertyTrace>()
                .RuleFor(t => t.DateSale, f => f.Date.Past(10))
                .RuleFor(t => t.Name, f => f.Company.CompanyName())
                .RuleFor(t => t.Value, f => f.Random.Decimal(10000, 1000000))
                .RuleFor(t => t.Tax, f => f.Random.Decimal(0, 50000));
        }

        public IEnumerable<Owner> GenerateOwners(int count)
        {
            var owners = _ownerFaker.Generate(count);
            return owners;
        }

        public IEnumerable<Property> GenerateProperties(int count, IEnumerable<string> ownerIds)
        {
            var ownerIdList = ownerIds.ToList();
            var properties = _propertyFaker
                .RuleFor(p => p.IdOwner, f => f.PickRandom(ownerIdList))
                .CustomInstantiator(f => new Property(
                    f.Company.CompanyName() + " Property",
                    f.Address.FullAddress(),
                    f.Random.Decimal(50000, 5000000),
                    f.Random.AlphaNumeric(10).ToUpper(),
                    f.Random.Int(1900, DateTime.Now.Year),
                    f.PickRandom(ownerIdList)))
                .Generate(count);

            return properties;
        }

        public IEnumerable<PropertyImage> GeneratePropertyImages(int count, IEnumerable<string> propertyIds)
        {
            var propertyIdList = propertyIds.ToList();
            var propertyImages = _propertyImageFaker
                .RuleFor(i => i.IdProperty, f => f.PickRandom(propertyIdList))
                .CustomInstantiator(f => new PropertyImage(
                    f.PickRandom(propertyIdList),
                    f.Image.PicsumUrl(),
                    f.Random.Bool(0.8f)))
                .Generate(count);

            return propertyImages;
        }

        public IEnumerable<PropertyTrace> GeneratePropertyTraces(int count, IEnumerable<string> propertyIds)
        {
            var propertyIdList = propertyIds.ToList();
            var propertyTraces = _propertyTraceFaker
                .RuleFor(t => t.IdProperty, f => f.PickRandom(propertyIdList))
                .CustomInstantiator(f => new PropertyTrace(
                    f.PickRandom(propertyIdList),
                    f.Date.Past(10),
                    f.Company.CompanyName(),
                    f.Random.Decimal(10000, 1000000),
                    f.Random.Decimal(0, 50000)))
                .Generate(count);

            return propertyTraces;
        }

    }
}