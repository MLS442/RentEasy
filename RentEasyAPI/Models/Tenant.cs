namespace RentEasyAPI.Models
{
    public class Tenant
    {
        public int TenantId { get; set; }

        public string FullName { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public DateOnly BirthDate { get; set; }

        public int PropertyId { get; set; }

        public Property? Property { get; set; }

        public ICollection<Ticket>? Tickets { get; set; }
    }
}
