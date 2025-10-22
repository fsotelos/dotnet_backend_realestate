using System;

namespace RealEstate.Application.DTOs
{
    public class PropertyTraceDto
    {
        public string Id { get; set; }
        public string IdProperty { get; set; }
        public DateTime DateSale { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
        public decimal Tax { get; set; }
        public PropertyDto Property { get; set; }
    }
}