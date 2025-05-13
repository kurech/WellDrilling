namespace robert.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string Role { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime RegistrationDate { get; set; }
        public bool? IsBlocked { get; set; }
        public List<WellUser> WellUsers { get; set; }
    }
}
