using Microsoft.EntityFrameworkCore;
using robert.Models;

namespace robert.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Well> Wells { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<WorkOrder> WorkOrders { get; set; }
        public DbSet<WorkSchedule> WorkSchedules { get; set; }
        public DbSet<WellUser> WellUsers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    }
}
