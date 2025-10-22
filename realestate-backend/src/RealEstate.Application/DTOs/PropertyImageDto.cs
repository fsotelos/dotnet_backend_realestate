namespace RealEstate.Application.DTOs
{
    public class PropertyImageDto
    {
        public string Id { get; set; }
        public string IdProperty { get; set; }
        public string File { get; set; }
        public bool Enabled { get; set; }
        public PropertyDto Property { get; set; }
    }
}