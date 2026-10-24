namespace RentEasyAPI.Responses
{
    public class TicketResponse
    {
        public int TicketId { get; set; }
        public required string Subject { get; set; }
        public required string Description { get; set; }
        public required string Status { get; set; }
        public required string TenantFullName { get; set; }
    }
}
