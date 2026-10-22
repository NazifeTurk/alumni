using Alumni.Data.Factories;
using Alumni.Models;
using Microsoft.EntityFrameworkCore;

namespace Alumni.Data;

public static class DatabaseSeeder
{
    public static void Seed(AlumniDbContext context)
    {
        // 1. Seed / Ensure Users exist
        var defaultUsers = UserFactory.CreateDefaultAlumni();
        foreach (var user in defaultUsers)
        {
            if (!context.Users.Any(u => u.Id == user.Id || u.Email == user.Email))
            {
                context.Users.Add(user);
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
