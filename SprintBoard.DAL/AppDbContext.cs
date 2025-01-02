using Microsoft.EntityFrameworkCore;
using SprintBoard.Entities;
using Entities = SprintBoard.Entities;

namespace SprintBoard.DAL
{
    public class AppDbContext : DbContext
    {
        public DbSet<Entities.User> Users { get; set; }
        public DbSet<Entities.Task> Tasks { get; set; }
        public DbSet<ErrorLog> ErrorLogs { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Seed data or configure entities here if needed
        }
    }
}

