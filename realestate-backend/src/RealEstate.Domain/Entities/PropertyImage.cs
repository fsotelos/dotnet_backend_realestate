using System;

namespace RealEstate.Domain.Entities
{
    public class PropertyImage
    {
        public string Id { get; private set; }
        public string IdProperty { get; private set; }
        public string File { get; private set; }
        public bool Enabled { get; private set; }

        public PropertyImage() { } // For EF Core

        public PropertyImage(string idProperty, string file, bool enabled = true)
        {
            Id = Guid.NewGuid().ToString();
            UpdatePropertyId(idProperty);
            UpdateFile(file);
            Enabled = enabled;
        }

        public void UpdatePropertyId(string idProperty)
        {
            if (string.IsNullOrWhiteSpace(idProperty))
                throw new ArgumentException("Property ID cannot be empty", nameof(idProperty));

            IdProperty = idProperty.Trim();
        }

        public void UpdateFile(string file)
        {
            if (string.IsNullOrWhiteSpace(file))
                throw new ArgumentException("File cannot be empty", nameof(file));
            if (!Uri.IsWellFormedUriString(file, UriKind.Absolute))
                throw new ArgumentException("File must be a valid URL", nameof(file));

            File = file.Trim();
        }

        public void Enable()
        {
            Enabled = true;
        }

        public void Disable()
        {
            Enabled = false;
        }

        public void ToggleEnabled()
        {
            Enabled = !Enabled;
        }
    }
}