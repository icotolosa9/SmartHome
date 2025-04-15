namespace Domain
{
    public class HomeOwnerPermission
    {
        public Guid Id { get; set; } 
        public Guid HomeId { get; set; }
        public Guid HomeOwnerId { get; set; }
        public string Permission { get; set; } = string.Empty;
        public Home? Home { get; set; }
        public User? HomeOwner { get; set; }
        public bool IsNotificationEnabled { get; set; } = false;
    }
}