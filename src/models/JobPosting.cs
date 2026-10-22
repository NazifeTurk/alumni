using System.Text.Json.Serialization;

namespace Alumni.Models;

public class JobPosting
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public string Location { get; set; } = "Remote";
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;

    // Navigation Properties
    [JsonIgnore]
    public User? User { get; set; }

    [JsonIgnore]
    public ICollection<Application> Applications { get; set; } = new List<Application>();
}

public record CreateJobPostingRequest(
    string Title,
    string Company,
    string? Location,
    string? Description
);

public record UpdateJobPostingRequest(
    string Title,
    string Company,
    string Location,
    string Description,
    bool IsActive
);
