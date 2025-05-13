namespace robert.Models
{
    public class Well
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Depth { get; set; }
        public double Diameter { get; set; }
        public string? DrillingMethod { get; set; }
        public string? SoilType { get; set; }

        //public List<WorkSchedule> WorkSchedules { get; set; }
        public List<WellUser> WellUsers { get; set; }
    }
}
