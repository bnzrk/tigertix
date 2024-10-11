using Microsoft.EntityFrameworkCore;
using TigerTix.Web.Data.Entities;

namespace TigerTix.Web.Data
{
    public class TigerTixContext : DbContext
    {
        public TigerTixContext(IConfiguration config)
        {
            _config = config;
        }

        public DbSet<User> Users { get; set; }

        private readonly IConfiguration _config;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(_config["ConnectionStrings:DefaultConnection"]);
        }
    }
}
