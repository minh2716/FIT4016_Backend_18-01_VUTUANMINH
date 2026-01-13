using Microsoft.EntityFrameworkCore;

namespace SchoolManagement
{
    public class SchoolDbContext : DbContext
    {
        public DbSet<School> Schools => Set<School>();
        public DbSet<Student> Students => Set<Student>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    if (!optionsBuilder.IsConfigured)
    {
        optionsBuilder.UseSqlite("Data Source=schoolmanagement.db");
    }
}



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<School>(entity =>
            {
                entity.ToTable("schools");
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Name).IsRequired();
                entity.HasIndex(s => s.Name).IsUnique();
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.ToTable("students");
                entity.HasKey(s => s.Id);

                entity.Property(s => s.FullName).IsRequired();
                entity.Property(s => s.StudentId).IsRequired();
                entity.Property(s => s.Email).IsRequired();

                entity.HasIndex(s => s.StudentId).IsUnique();
                entity.HasIndex(s => s.Email).IsUnique();

                entity.HasOne(s => s.School)
                      .WithMany(sc => sc.Students)
                      .HasForeignKey(s => s.SchoolId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
