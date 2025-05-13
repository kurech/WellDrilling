namespace robert.Models
{
    public class WellUser
    {
        public int Id { get; set; }
        public int WellId { get; set; }
        public int UserId { get; set; }
        public int WorkScheduleId { get; set; }
        public bool IsReady { get; set; }
        public Well Well { get; set; }
        public User User { get; set; }
        public WorkSchedule WorkSchedule { get; set; }
    }
}
