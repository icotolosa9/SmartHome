using Domain;

namespace Models.Out
{
    public class CreateHomeResponse
    {
        public Guid Id { get; set; }
        public string Address { get; set; }
        public string Location { get; set; }
        public string Owner { get; set; } 
        public int Capacity { get; set; }

        public CreateHomeResponse() { } 

        public CreateHomeResponse(Home home)
        {
            Id = home.Id;
            Address = home.Address;
            Location = home.Location;
            Owner = $"{home.Owner.FirstName} {home.Owner.LastName}";
            Capacity = home.Capacity;
        }

        public override bool Equals(object obj)
        {
            return obj is CreateHomeResponse response &&
                   Address == response.Address;
        }
    }
}