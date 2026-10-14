using Alumni.Models;

namespace Alumni.Data.Factories;

public static class AnnouncementFactory
{
    public static Announcement Create(string title, string content, string category = "General", string author = "Alumni Office", bool isActive = true)
    {
        return new Announcement
        {
            Title = title.Trim(),
            Content = content.Trim(),
            Category = category,
            Author = author,
            IsActive = isActive,
            CreatedAt = DateTime.UtcNow
        };
    }

    public static List<Announcement> CreateDefaultAnnouncements()
    {
        return new List<Announcement>
        {
            new()
            {
                Id = 1,
                Title = "2026 Annual Alumni Homecoming Gala",
                Content = "Join fellow Istanbul University graduates for our annual dinner and networking keynote at the Central Campus.",
                Category = "Reunion",
                Author = "Alumni Association",
                CreatedAt = new DateTime(2026, 5, 1, 0, 0, 0, DateTimeKind.Utc),
                IsActive = true
            },
            new()
            {
                Id = 2,
                Title = "Tech Mentorship Cohort 2026 Applications Open",
                Content = "Experienced engineering alumni are invited to mentor final-year students preparing for software engineering roles.",
                Category = "Mentorship",
                Author = "Career Center",
                CreatedAt = new DateTime(2026, 5, 10, 0, 0, 0, DateTimeKind.Utc),
                IsActive = true
            }
        };
    }
}
