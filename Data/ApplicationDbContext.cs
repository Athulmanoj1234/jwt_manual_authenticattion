
using jwtmanualauthentication.Models.Enities;
using Microsoft.EntityFrameworkCore;


namespace jwtmanualauthentication.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {
//            This means call the base class constructor(DbContext's constructor) and pass it the options.
//In other words, you’re telling EF Core’s base DbContext to initialize itself using the provided options.
            
        }

        public DbSet<User> Users { get; set; }
    }
}
