using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.DataContext
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
       
        public DbSet<Player> Players { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamAssignment> TeamAssignments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>().ToTable("Player");
            modelBuilder.Entity<Team>().ToTable("Team");
            modelBuilder.Entity<TeamAssignment>().ToTable("TeamAssignment");

            modelBuilder.Entity<TeamAssignment>().HasKey(t => new { t.PlayerId, t.TeamId });
        }
    }
}
