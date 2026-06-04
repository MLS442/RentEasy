namespace RentEasyAPI.Models
{
    public class Landlord
    {
        public int LandlordId { get; set; }
        public string FullName { get; set; }
        public ICollection<Property>? Properties { get; set; }
    }
}
