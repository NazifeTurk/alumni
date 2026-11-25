using Alumni.Data.Factories;
using Alumni.Controllers;
using Alumni.Models;
using Microsoft.EntityFrameworkCore;

namespace Alumni.Data;

public static class DatabaseSeeder
{
    public static void Seed(AlumniDbContext context)
    {
        // 1. Ensure admin account exists with correct password hash
        var adminEmail = "admin@iu.edu.tr";
        var adminUser = context.Users.FirstOrDefault(u => u.Email == adminEmail);
        if (adminUser == null)
        {
            context.Users.Add(new User
            {
                Name = "Antigravity Admin",
                Email = adminEmail,
                Department = "Sistem Yönetimi",
                GraduationYear = 2020,
                Role = "admin",
                PasswordHash = AuthController.HashPassword("admin1453"),
                CreatedAt = DateTime.UtcNow
            });
            context.SaveChanges();
            Console.WriteLine("[DatabaseSeeder] Admin user created: admin@iu.edu.tr / admin1453");
        }
        else if (adminUser.PasswordHash.StartsWith("$2a$") || string.IsNullOrEmpty(adminUser.PasswordHash))
        {
            adminUser.Role = "admin";
            adminUser.PasswordHash = AuthController.HashPassword("admin1453");
            adminUser.UpdatedAt = DateTime.UtcNow;
            context.SaveChanges();
            Console.WriteLine("[DatabaseSeeder] Admin password hash updated.");
        }

        // 2. Seed default alumni users (fix placeholder hashes)
        var defaultAlumni = new[]
        {
            (Id: 42, Name: "Elif Kaya",     Email: "elif.kaya@ogr.iu.edu.tr",     Dept: "Bilgisayar Mühendisliği", Year: 2024),
            (Id: 43, Name: "Mert Aydın",    Email: "mert.aydin@ogr.iu.edu.tr",    Dept: "İşletme",                 Year: 2021),
            (Id: 44, Name: "Zeynep Arslan", Email: "zeynep.arslan@ogr.iu.edu.tr", Dept: "Hukuk",                   Year: 2019),
            (Id: 45, Name: "Can Öztürk",    Email: "can.ozturk@ogr.iu.edu.tr",    Dept: "Psikoloji",               Year: 2023),
        };
        var defaultHash = AuthController.HashPassword("alumni123");
        foreach (var a in defaultAlumni)
        {
            var existing = context.Users.FirstOrDefault(u => u.Email == a.Email);
            if (existing == null)
            {
                context.Users.Add(new User
                {
                    Name = a.Name, Email = a.Email, Department = a.Dept,
                    GraduationYear = a.Year, Role = "alumni",
                    PasswordHash = defaultHash,
                    CreatedAt = DateTime.UtcNow
                });
            }
            else if (existing.PasswordHash.StartsWith("$2a$") || string.IsNullOrEmpty(existing.PasswordHash))
            {
                existing.PasswordHash = defaultHash;
            }
        }
        context.SaveChanges();

        // 2. Seed Announcements if table is empty
        if (!context.Announcements.Any())
        {
            var defaultAnnouncements = AnnouncementFactory.CreateDefaultAnnouncements();
            context.Announcements.AddRange(defaultAnnouncements);
            context.SaveChanges();
            Console.WriteLine($"[DatabaseSeeder] Seeded {defaultAnnouncements.Count} default announcements via AnnouncementFactory.");
        }

        // 3. Seed Job Postings (Week 06: Slide Section A Entities)
        if (!context.JobPostings.Any())
        {
            var slideJobs = JobPostingFactory.CreateSlideJobPostings();
            context.JobPostings.AddRange(slideJobs);
            context.SaveChanges();
            Console.WriteLine($"[DatabaseSeeder] Seeded {slideJobs.Count} default job postings via JobPostingFactory.");
        }

        // 4. Seed Applications (Week 06: Many-to-Many Join Table)
        if (!context.Applications.Any())
        {
            var slideApplications = new List<Application>
            {
                // Elif (3) applied to posting 10
                new() { UserId = 3, JobPostingId = 10, Status = "applied", CreatedAt = DateTime.UtcNow.AddDays(-2) },
                // Can (2) applied to posting 10 (posting 10 has two applicants)
                new() { UserId = 2, JobPostingId = 10, Status = "applied", CreatedAt = DateTime.UtcNow.AddDays(-1) },
                // Elif (3) applied to posting 12
                new() { UserId = 3, JobPostingId = 12, Status = "interview", CreatedAt = DateTime.UtcNow.AddDays(-1) }
            };

            context.Applications.AddRange(slideApplications);
            context.SaveChanges();
            Console.WriteLine($"[DatabaseSeeder] Seeded {slideApplications.Count} slide applications (Elif & Can).");
        }

        // 5. Synchronize PostgreSQL identity sequences
        SyncSequence(context, "users", "Id");
        SyncSequence(context, "announcements", "Id");
        SyncSequence(context, "job_postings", "Id");
        SyncSequence(context, "applications", "Id");
    }

    private static void SyncSequence(AlumniDbContext context, string tableName, string idColumn)
    {
        try
        {
            context.Database.ExecuteSqlRaw($@"
                SELECT setval(
                    pg_get_serial_sequence('{tableName}', '{idColumn}'),
                    GREATEST((SELECT COALESCE(MAX(""{idColumn}""), 0) FROM {tableName}) + 1, 1),
                    false);");
        }
        catch
        {
            // Non-Postgres fallback / ignore if sequence is not present
        }
    }
}
