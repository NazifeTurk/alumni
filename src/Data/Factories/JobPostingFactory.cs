using Alumni.Models;

namespace Alumni.Data.Factories;

public static class JobPostingFactory
{
    public static JobPosting Create(int userId, string title, string company, string location = "Remote", string? description = null)
    {
        return new JobPosting
        {
            UserId = userId,
            Title = title.Trim(),
            Company = company.Trim(),
            Location = location.Trim(),
            Description = description?.Trim() ?? $"{title} position at {company}",
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };
    }

    public static List<JobPosting> CreateSlideJobPostings()
    {
        return new List<JobPosting>
        {
            new()
            {
                Id = 10,
                UserId = 1,
                Title = "Backend Intern",
                Company = "Tech Corp",
                Location = "Istanbul / Hybrid",
                Description = "Exciting backend engineering internship working with C# and cloud microservices.",
                CreatedAt = DateTime.UtcNow.AddDays(-10),
                IsActive = true
            },
            new()
            {
                Id = 11,
                UserId = 1,
                Title = "Data Analyst",
                Company = "Data Insights",
                Location = "Remote",
                Description = "Longitudinal telemetry and analytics role supporting strategic business decisions.",
                CreatedAt = DateTime.UtcNow.AddDays(-5),
                IsActive = true
            },
            new()
            {
                Id = 12,
                UserId = 2,
                Title = "QA Tester",
                Company = "TestLab",
                Location = "Istanbul",
                Description = "Quality assurance automation and integration test engineering.",
                CreatedAt = DateTime.UtcNow.AddDays(-3),
                IsActive = true
            }
        };
    }
}
