using System.Collections.Generic;

namespace RealEstate.Application.DTOs
{
    public class PaginatedPropertyDto
    {
        public IEnumerable<PropertyDto> Properties { get; set; }
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}