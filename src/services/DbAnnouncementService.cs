using Alumni.Data;
using Alumni.Models;

namespace Alumni.Services;

public class DbAnnouncementService : IAnnouncementService
{
    private readonly AlumniDbContext _context;

    public DbAnnouncementService(AlumniDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Announcement> GetAll()
    {
        return _context.Announcements.OrderByDescending(a => a.CreatedAt).ToList();
    }

    public Announcement? GetById(int id)
    {
        return _context.Announcements.Find(id);
    }

    public Announcement Create(CreateAnnouncementRequest request)
    {
        var announcement = new Announcement
        {
            Title = request.Title.Trim(),
            Content = request.Content.Trim(),
            Category = !string.IsNullOrWhiteSpace(request.Category) ? request.Category.Trim() : "General",
            Author = !string.IsNullOrWhiteSpace(request.Author) ? request.Author.Trim() : "Alumni Office",
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _context.Announcements.Add(announcement);
        _context.SaveChanges();
        return announcement;
    }

    public Announcement? Update(int id, UpdateAnnouncementRequest request)
    {
        var announcement = _context.Announcements.Find(id);
        if (announcement == null) return null;

        announcement.Title = request.Title.Trim();
        announcement.Content = request.Content.Trim();
        announcement.Category = !string.IsNullOrWhiteSpace(request.Category) ? request.Category.Trim() : "General";
        announcement.Author = !string.IsNullOrWhiteSpace(request.Author) ? request.Author.Trim() : "Alumni Office";
        announcement.IsActive = request.IsActive;

        _context.SaveChanges();
        return announcement;
    }

    public bool Delete(int id)
    {
        var announcement = _context.Announcements.Find(id);
        if (announcement == null) return false;

        _context.Announcements.Remove(announcement);
        _context.SaveChanges();
        return true;
    }
}
