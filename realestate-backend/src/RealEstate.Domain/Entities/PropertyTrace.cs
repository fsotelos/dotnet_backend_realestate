namespace RealEstate.Domain.Entities
{
    public class PropertyTrace
    {
        public string Id { get; private set; }
        public string IdProperty { get; private set; }
        public DateTime DateSale { get; private set; }
        public string Name { get; private set; }
        public decimal Value { get; private set; }
        public decimal Tax { get; private set; }

        public PropertyTrace() { } // For EF Core

        public PropertyTrace(string idProperty, DateTime dateSale, string name, decimal value, decimal tax)
        {
            Id = Guid.NewGuid().ToString();
            UpdatePropertyId(idProperty);
            UpdateDateSale(dateSale);
            UpdateName(name);
            UpdateValue(value);
            UpdateTax(tax);
        }

        public void UpdatePropertyId(string idProperty)
        {
            if (string.IsNullOrWhiteSpace(idProperty))
                throw new ArgumentException("Property ID cannot be empty", nameof(idProperty));

            IdProperty = idProperty.Trim();
        }

        public void UpdateDateSale(DateTime dateSale)
        {
            if (dateSale > DateTime.Now)
                throw new ArgumentException("Sale date cannot be in the future", nameof(dateSale));

            DateSale = dateSale;
        }

        public void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty", nameof(name));
            if (name.Length > 100)
                throw new ArgumentException("Name cannot exceed 100 characters", nameof(name));

            Name = name.Trim();
        }

        public void UpdateValue(decimal value)
        {
            if (value <= 0)
                throw new ArgumentException("Value must be greater than zero", nameof(value));

            Value = value;
        }

        public void UpdateTax(decimal tax)
        {
            if (tax < 0)
                throw new ArgumentException("Tax cannot be negative", nameof(tax));

            Tax = tax;
        }

        public decimal GetTotalAmount()
        {
            return Value + Tax;
        }

        public decimal GetTaxPercentage()
        {
            return Value > 0 ? (Tax / Value) * 100 : 0;
        }
    }
}