using Microsoft.EntityFrameworkCore;
using TigerTix.Web.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace TigerTix.Web.Data
{
    public class TigerTixContext : IdentityDbContext<ApplicationUser>
    {
        public TigerTixContext(IConfiguration config)
        {
            _config = config;
        }

        public DbSet<Event> Events { get; set; }

        public DbSet<Ticket> Tickets { get; set; }

        private readonly IConfiguration _config;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(_config["ConnectionStrings:LocalConnection"]);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Event>()
                .HasMany(e => e.Tickets)
                .WithOne(t => t.Event)
                .HasForeignKey(t => t.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(builder);
        }
    }
}
