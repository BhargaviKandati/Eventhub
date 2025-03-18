using Microsoft.EntityFrameworkCore;

namespace Eventhub.Models
{
    public class EFCoreDBContext : DbContext
    {
        public EFCoreDBContext(DbContextOptions<EFCoreDBContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Venue> Venues { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Category> Categories { get; set; }

    }
}
