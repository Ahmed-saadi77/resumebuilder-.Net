using Microsoft.EntityFrameworkCore;
using ResumeBuilder.Models;


namespace ResumeBuilder.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Resume> Resumes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Resumes)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId);
        }
    }
}
