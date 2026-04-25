using Microsoft.EntityFrameworkCore;
using RentEasyAPI.Models;

namespace RentEasyAPI.Data
{
    public class RentEasyContext : DbContext
    {
        public RentEasyContext(DbContextOptions<RentEasyContext> options) : base(options)
        {

        }

        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<Landlord> Landlords { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
