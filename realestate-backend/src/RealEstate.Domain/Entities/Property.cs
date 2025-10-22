namespace RealEstate.Domain.Entities
{
    public class Property
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public decimal Price { get; set; }
        public string CodeInternal { get; set; }
        public int Year { get; set; }
        public string IdOwner { get; set; }
        public Owner Owner { get; set; }

        private readonly List<PropertyImage> _images = new();
        public IReadOnlyCollection<PropertyImage> Images => _images.AsReadOnly();

        private readonly List<PropertyTrace> _traces = new();
        public IReadOnlyCollection<PropertyTrace> Traces => _traces.AsReadOnly();

        public Property() { } // For EF Core

        public Property(string name, string address, decimal price, string codeInternal, int year, string idOwner)
        {
            Id = Guid.NewGuid().ToString();
            UpdateName(name);
            UpdateAddress(address);
            UpdatePrice(price);
            UpdateCodeInternal(codeInternal);
            UpdateYear(year);
            UpdateOwnerId(idOwner);
        }

        public void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty", nameof(name));
            if (name.Length > 100)
                throw new ArgumentException("Name cannot exceed 100 characters", nameof(name));

            Name = name.Trim();
        }

        public void UpdateAddress(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                throw new ArgumentException("Address cannot be empty", nameof(address));
            if (address.Length > 200)
                throw new ArgumentException("Address cannot exceed 200 characters", nameof(address));

            Address = address.Trim();
        }

        public void UpdatePrice(decimal price)
        {
            if (price <= 0)
                throw new ArgumentException("Price must be greater than zero", nameof(price));
            if (price > 1_000_000_000)
                throw new ArgumentException("Price cannot exceed 1 billion", nameof(price));

            Price = price;
        }

        public void UpdateCodeInternal(string codeInternal)
        {
            if (string.IsNullOrWhiteSpace(codeInternal))
                throw new ArgumentException("CodeInternal cannot be empty", nameof(codeInternal));
            if (codeInternal.Length > 20)
                throw new ArgumentException("CodeInternal cannot exceed 20 characters", nameof(codeInternal));

            CodeInternal = codeInternal.Trim().ToUpper();
        }

        public void UpdateYear(int year)
        {
            var currentYear = DateTime.Now.Year;
            if (year < 1800 || year > currentYear + 1)
                throw new ArgumentException($"Year must be between 1800 and {currentYear + 1}", nameof(year));

            Year = year;
        }

        public void UpdateOwnerId(string idOwner)
        {
            if (string.IsNullOrWhiteSpace(idOwner))
                throw new ArgumentException("Owner ID cannot be empty", nameof(idOwner));

            IdOwner = idOwner.Trim();
        }

        public void AddImage(string file, bool enabled = true)
        {
            if (string.IsNullOrWhiteSpace(file))
                throw new ArgumentException("Image file cannot be empty", nameof(file));

            var image = new PropertyImage(Id, file, enabled);
            _images.Add(image);
        }

        public void RemoveImage(string imageId)
        {
            var image = _images.FirstOrDefault(i => i.Id == imageId);
            if (image != null)
                _images.Remove(image);
        }

        public void AddTrace(DateTime dateSale, string name, decimal value, decimal tax)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Trace name cannot be empty", nameof(name));
            if (value <= 0)
                throw new ArgumentException("Value must be greater than zero", nameof(value));
            if (tax < 0)
                throw new ArgumentException("Tax cannot be negative", nameof(tax));

            var trace = new PropertyTrace(Id, dateSale, name, value, tax);
            _traces.Add(trace);
        }

        public decimal GetTotalTracesValue()
        {
            return _traces.Sum(t => t.Value);
        }

        public int GetPropertyAge()
        {
            return DateTime.Now.Year - Year;
        }
    }
    public class PropertyWithImages : Property
    {
       
          
            public List<PropertyImage> Images { get; set; } = new();
   
       
    }
}