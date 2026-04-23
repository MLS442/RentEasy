namespace RentEasyAPI.Models
{
    public class Property
    {
        public int PropertyId { get; set; }

        public string Address { get; set; }

        public decimal Price { get; set; }

        public int Bedrooms { get; set; }

        public DateOnly? LeaseEndDate { get; set; }

        public ICollection<Tenant>? Tenants { get; set; }
        public int LandlordId { get; set; }
        public Landlord? Landlord { get; set; }
    }
}
