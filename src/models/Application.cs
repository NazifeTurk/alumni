using System.Text.Json.Serialization;

namespace Alumni.Models;

public class Application
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int JobPostingId { get; set; }
    public string Status { get; set; } = "applied"; // applied, interview, accepted, rejected
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    [JsonIgnore]
    public User? User { get; set; }

    [JsonIgnore]
    public JobPosting? JobPosting { get; set; }
}

public record CreateApplicationRequest(
    int UserId
);

public record UpdateApplicationStatusRequest(
    string Status
);
