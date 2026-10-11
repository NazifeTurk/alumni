using System.Collections.Concurrent;
using Alumni.Models;

namespace Alumni.Services;

public class InMemoryAnnouncementService : IAnnouncementService
{
    private readonly ConcurrentDictionary<int, Announcement> _store = new();
    private int _nextId = 1;

    public InMemoryAnnouncementService()
    {
        // Pre-seed initial announcements
        Create(new CreateAnnouncementRequest(
            Title: "2026 Annual Alumni Homecoming Gala",
            Content: "Join fellow Istanbul University graduates for our annual dinner and networking keynote at the Central Campus.",
            Category: "Reunion",
            Author: "Alumni Association"
        ));

        Create(new CreateAnnouncementRequest(
            Title: "Tech Mentorship Cohort 2026 Applications Open",
            Content: "Experienced engineering alumni are invited to mentor final-year students preparing for software engineering roles.",
            Category: "Mentorship",
            Author: "Career Center"
        ));

        Create(new CreateAnnouncementRequest(
            Title: "Workshop: Building Scalable Cloud Backends",
            Content: "Free weekend technical seminar hosted by distinguished alumni working in cloud architecture.",
            Category: "Career",
            Author: "Computer Science Faculty"
        ));
    }

    public IEnumerable<Announcement> GetAll()
    {
        return _store.Values.OrderByDescending(a => a.CreatedAt);
    }

    public Announcement? GetById(int id)
    {
        _store.TryGetValue(id, out var announcement);
        return announcement;
    }

    public Announcement Create(CreateAnnouncementRequest request)
    {
        int id = Interlocked.Increment(ref _nextId);
        var announcement = new Announcement
        {
            Id = id,
            Title = request.Title,
            Content = request.Content,
            Category = string.IsNullOrWhiteSpace(request.Category) ? "General" : request.Category,
            Author = string.IsNullOrWhiteSpace(request.Author) ? "Alumni Office" : request.Author,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        _store[id] = announcement;
        return announcement;
    }

    public Announcement? Update(int id, UpdateAnnouncementRequest request)
    {
        if (!_store.TryGetValue(id, out var existing))
        {
            return null;
        }

        existing.Title = request.Title;
        existing.Content = request.Content;
        existing.Category = request.Category;
        existing.Author = request.Author;
        existing.IsActive = request.IsActive;

        return existing;
    }

    public bool Delete(int id)
    {
        return _store.TryRemove(id, out _);
    }
}
