using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<Animal> Animals { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Favorite> Favorites { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Favorite>()
                .HasOne(a=>a.Animal)
                .WithMany(a=>a.Favorite)
                .HasForeignKey(a=>a.AnimalId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Favorite>()
                .HasOne(u=>u.User)
                .WithMany(u=>u.Favorite)
                .HasForeignKey(u=>u.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
