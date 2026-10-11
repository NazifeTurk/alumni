namespace Alumni.Models;

public class Announcement
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Category { get; set; } = "General"; // Career, Reunion, Academic, General
    public string Author { get; set; } = "Alumni Office";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;
}

public record CreateAnnouncementRequest(
    string Title,
    string Content,
    string? Category,
    string? Author
);

public record UpdateAnnouncementRequest(
    string Title,
    string Content,
    string Category,
    string Author,
    bool IsActive
);
