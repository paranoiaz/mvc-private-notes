using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Models;

namespace Data
{
    public class DatabaseContext : IdentityDbContext
    {
        public DbSet<User> _Users { get; set; }
        public DbSet<Note> _Notes { get; set; }

        // only used for passing options to constructor
        public DatabaseContext(DbContextOptions options) : base(options)
        {

        }
    }
}