using System.Text.RegularExpressions;

namespace Domain
{
    public class Home
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public Guid HomeOwnerId { get; set; }
        public User? Owner { get; set; }
        public string Address { get; set; }
        public string Location { get; set; }
        public List<User>? Members { get; set; } = new List<User>();
        public int Capacity { get; set; }
        public List<HomeOwnerPermission>? MemberPermissions { get; set; } = new List<HomeOwnerPermission>();
        public List<HomeDevice>? HomeDevices { get; set; } = new List<HomeDevice>();
        public List<Room> Rooms { get; set; } = new List<Room>();

        public Home() { }

        public Home(string? name, string address, string location, int capacity)
        {
            ValidateInputs(address, location, capacity);

            Id = Guid.NewGuid();
            Name = name;
            Address = address;
            Location = location;
            Capacity = capacity;
        }

        private void ValidateInputs(string address, string location, int capacity)
        {
            if (string.IsNullOrEmpty(address))
            {
                throw new ArgumentException("La dirección es requerida.");
            }

            if (string.IsNullOrEmpty(location))
            {
                throw new ArgumentException("La ubicación es requerida.");
            }
            if (capacity <= 0)
            {
                throw new ArgumentException("La capacidad debe ser mayor a 0.");
            }
        }
    }
}