namespace robert.Models
{
    public class WorkSchedule
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double PlannedCost { get; set; }
        public required string Status { get; set; }
        public List<WellUser> WellUsers { get; set; }
    }
}
