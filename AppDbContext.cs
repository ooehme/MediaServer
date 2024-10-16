using Microsoft.EntityFrameworkCore;
using MediaServer.Models;

namespace MediaServer.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<MediaFile> MediaFiles { get; set; }
        public DbSet<VideoFile> VideoFiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MediaFile>().HasKey(m => m.Path);
            modelBuilder.Entity<VideoFile>().HasKey(v => v.Path);
        }
    }
}