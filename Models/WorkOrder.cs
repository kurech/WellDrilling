namespace robert.Models
{
    public class WorkOrder
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int WellId { get; set; }
        public DateTime OrderDate { get; set; }
        public required string Description { get; set; }
        public double Cost { get; set; }
        public int? WorkScheduleId { get; set; }

        public Client Client { get; set; }
        public Well Well { get; set; }
        public WorkSchedule WorkSchedule { get; set; }
    }
}
