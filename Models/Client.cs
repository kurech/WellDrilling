using Microsoft.Data.SqlClient;

namespace robert.Models
{
    public class Client
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string ContactNumber { get; set; }
        public required string Email { get; set; }
        public required string Address { get; set; }
        public required ClientType ClientType { get; set; }
        public DateTime RegistrationDate { get; set; }
        public bool IsDeleted { get; set; }

        public List<WorkOrder> WorkOrders { get; set; }
    }

    public enum ClientType
    {
        Физическое, Юридическое
    }
}
