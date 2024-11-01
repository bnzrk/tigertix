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

        private readonly IConfiguration _config;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(_config["ConnectionStrings:LocalConnection"]);
        }
    }
}
