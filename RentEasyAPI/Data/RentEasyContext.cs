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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.Landlord)
                .WithOne()
                .HasForeignKey<User>(u => u.LandlordId);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Tenant)
                .WithOne()
                .HasForeignKey<User>(u => u.TenantId);
        }
    }    
}
