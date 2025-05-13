namespace robert.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public required string NotificationType { get; set; }
        public DateTime DateTime { get; set; }
        public string? Content { get; set; }
        public bool IsRead { get; set; }

        public User User { get; set; }
    }
}
