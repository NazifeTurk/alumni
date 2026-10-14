using Alumni.Models;
using Microsoft.EntityFrameworkCore;

namespace Alumni.Data;

public class AlumniDbContext : DbContext
{
    public AlumniDbContext(DbContextOptions<AlumniDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Announcement> Announcements => Set<Announcement>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Users table
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Name).IsRequired().HasMaxLength(150);
            entity.Property(u => u.Email).IsRequired().HasMaxLength(150);
            entity.Property(u => u.Role).HasMaxLength(50).HasDefaultValue("alumni");
            entity.Property(u => u.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        // Configure Announcements table
        modelBuilder.Entity<Announcement>(entity =>
        {
            entity.ToTable("announcements");
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Title).IsRequired().HasMaxLength(200);
            entity.Property(a => a.Content).IsRequired();
            entity.Property(a => a.Category).HasMaxLength(50).HasDefaultValue("General");
            entity.Property(a => a.Author).HasMaxLength(100).HasDefaultValue("Alumni Office");
            entity.Property(a => a.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        // Seed initial data if tables are freshly created
        modelBuilder.Entity<User>().HasData(
            new User { Id = 42, Name = "Elif Kaya", Email = "elif.kaya@ogr.iu.edu.tr", GraduationYear = 2024, Role = "alumni", CreatedAt = new DateTime(2026, 1, 15, 0, 0, 0, DateTimeKind.Utc) },
            new User { Id = 43, Name = "Mert Aydın", Email = "mert.aydin@ogr.iu.edu.tr", GraduationYear = 2021, Role = "alumni", CreatedAt = new DateTime(2026, 2, 10, 0, 0, 0, DateTimeKind.Utc) },
            new User { Id = 44, Name = "Zeynep Arslan", Email = "zeynep.arslan@ogr.iu.edu.tr", GraduationYear = 2019, Role = "alumni", CreatedAt = new DateTime(2026, 3, 5, 0, 0, 0, DateTimeKind.Utc) },
            new User { Id = 45, Name = "Can Öztürk", Email = "can.ozturk@ogr.iu.edu.tr", GraduationYear = 2023, Role = "alumni", CreatedAt = new DateTime(2026, 4, 1, 0, 0, 0, DateTimeKind.Utc) }
        );

        modelBuilder.Entity<Announcement>().HasData(
            new Announcement
            {
                Id = 1,
                Title = "2026 Annual Alumni Homecoming Gala",
                Content = "Join fellow Istanbul University graduates for our annual dinner and networking keynote at the Central Campus.",
                Category = "Reunion",
                Author = "Alumni Association",
                CreatedAt = new DateTime(2026, 5, 1, 0, 0, 0, DateTimeKind.Utc),
                IsActive = true
            },
            new Announcement
            {
                Id = 2,
                Title = "Tech Mentorship Cohort 2026 Applications Open",
                Content = "Experienced engineering alumni are invited to mentor final-year students preparing for software engineering roles.",
                Category = "Mentorship",
                Author = "Career Center",
                CreatedAt = new DateTime(2026, 5, 10, 0, 0, 0, DateTimeKind.Utc),
                IsActive = true
            }
        );
    }
}
