using ATL_WebUI.Models.SQL;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Container = ATL_WebUI.Models.SQL.Container;

namespace ATL_WebUI.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Container> Containers { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Detail> Details { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<Shipment> Shipments { get; set; }
    }
}
