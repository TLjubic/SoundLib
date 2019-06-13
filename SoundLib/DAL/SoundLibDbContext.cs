using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Model;
using System;

namespace DAL
{
    public class SoundLibDbContext : IdentityDbContext<User>
    {

        public SoundLibDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Album> Albums { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<Genre> Genres { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Genre>().HasData(new Genre { Id = 1, Title = "Rock" });
            modelBuilder.Entity<Genre>().HasData(new Genre { Id = 2, Title = "Pop" });
            modelBuilder.Entity<Genre>().HasData(new Genre { Id = 3, Title = "Country" });
            modelBuilder.Entity<Genre>().HasData(new Genre { Id = 4, Title = "Jazz" });
            modelBuilder.Entity<Genre>().HasData(new Genre { Id = 5, Title = "Blues" });
            modelBuilder.Entity<Genre>().HasData(new Genre { Id = 6, Title = "R&B" });

        }

    }
}
