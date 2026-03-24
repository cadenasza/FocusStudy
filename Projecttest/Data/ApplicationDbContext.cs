using Microsoft.EntityFrameworkCore;
using Projecttest.Models;

namespace Projecttest.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<StudySession> StudySessions { get; set; }
    }
}
