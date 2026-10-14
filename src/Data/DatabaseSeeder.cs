using Alumni.Data.Factories;
using Microsoft.EntityFrameworkCore;

namespace Alumni.Data;

public static class DatabaseSeeder
{
    public static void Seed(AlumniDbContext context)
    {
        // 1. Seed Users if table is empty
        if (!context.Users.Any())
        {
            var defaultUsers = UserFactory.CreateDefaultAlumni();
            context.Users.AddRange(defaultUsers);
            context.SaveChanges();

            // Synchronize PostgreSQL identity sequence to avoid PK collision on next INSERT
            try
            {
                context.Database.ExecuteSqlRaw(@"
                    SELECT setval(
                        pg_get_serial_sequence('users', 'Id'),
                        GREATEST((SELECT COALESCE(MAX(""Id""), 0) FROM users) + 1, 1),
                        false);");
            }
            catch
            {
                // Non-Postgres fallback / ignore if sequence is not present
            }

            Console.WriteLine($"[DatabaseSeeder] Seeded {defaultUsers.Count} default users via UserFactory.");
        }

        // 2. Seed Announcements if table is empty
        if (!context.Announcements.Any())
        {
            var defaultAnnouncements = AnnouncementFactory.CreateDefaultAnnouncements();
            context.Announcements.AddRange(defaultAnnouncements);
            context.SaveChanges();

            try
            {
                context.Database.ExecuteSqlRaw(@"
                    SELECT setval(
                        pg_get_serial_sequence('announcements', 'Id'),
                        GREATEST((SELECT COALESCE(MAX(""Id""), 0) FROM announcements) + 1, 1),
                        false);");
            }
            catch
            {
                // Non-Postgres fallback
            }

            Console.WriteLine($"[DatabaseSeeder] Seeded {defaultAnnouncements.Count} default announcements via AnnouncementFactory.");
        }
    }
}
