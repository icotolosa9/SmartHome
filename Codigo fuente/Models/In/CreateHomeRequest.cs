using Domain;

namespace Models.In
{
    public class CreateHomeRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; }
        public string Location { get; set; }
        public int Capacity { get; set; }

        public Home ToEntity()
        {
            return new Home(Name, Address, Location, Capacity);
        }
    }
}