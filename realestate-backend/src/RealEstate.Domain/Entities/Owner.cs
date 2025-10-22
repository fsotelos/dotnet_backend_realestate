using System;

namespace RealEstate.Domain.Entities
{
    public class Owner
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public string Address { get; private set; }
        public string Photo { get; private set; }
        public DateTime? Birthday { get; private set; }

        public Owner() { }
        public Owner(string name, string address, string photo, DateTime? birthday)
        {
            Id = Guid.NewGuid().ToString();
            UpdateName(name);
            UpdateAddress(address);
            UpdatePhoto(photo);
            UpdateBirthday(birthday);
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

        public void UpdatePhoto(string photo)
        {
            if (!string.IsNullOrWhiteSpace(photo) && !Uri.IsWellFormedUriString(photo, UriKind.Absolute))
                throw new ArgumentException("Photo must be a valid URL", nameof(photo));

            Photo = photo?.Trim();
        }

        public void UpdateBirthday(DateTime? birthday)
        {
            if (birthday.HasValue && birthday.Value > DateTime.Now)
                throw new ArgumentException("Birthday cannot be in the future", nameof(birthday));

            Birthday = birthday;
        }

        public int GetAge()
        {
            if (!Birthday.HasValue) return 0;

            var today = DateTime.Today;
            var age = today.Year - Birthday.Value.Year;
            if (Birthday.Value.Date > today.AddYears(-age)) age--;

            return age;
        }
    }
}