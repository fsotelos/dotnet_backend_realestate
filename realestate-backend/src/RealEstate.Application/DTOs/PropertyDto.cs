using System;

namespace RealEstate.Application.DTOs
{
    public class PropertyDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public decimal Price { get; set; }
        public string CodeInternal { get; set; }
        public int Year { get; set; }
        public string IdOwner { get; set; }
        public OwnerDto Owner { get; set; }
        public string Image { get; set; }
    }
}