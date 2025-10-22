using System;
using System.Collections.Generic;

namespace RealEstate.Application.DTOs
{
    public class OwnerDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Photo { get; set; }
        public DateTime? Birthday { get; set; }
        public ICollection<PropertyDto> Properties { get; set; } = new List<PropertyDto>();
    }
}