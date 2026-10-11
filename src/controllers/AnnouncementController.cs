using Alumni.Models;
using Alumni.Services;
using Microsoft.AspNetCore.Mvc;

namespace Alumni.Controllers;

[Route("announcements")]
public class AnnouncementController : Controller
{
    private readonly IAnnouncementService _announcementService;

    public AnnouncementController(IAnnouncementService announcementService)
    {
        _announcementService = announcementService;
    }

    // GET /announcements -> List announcements HTML View
    [HttpGet("")]
    public IActionResult Index()
    {
        var announcements = _announcementService.GetAll();
        return Content(RenderAnnouncementListView(announcements), "text/html");
    }

    // POST /announcements -> Create announcement from HTML Form
    [HttpPost("")]
    public IActionResult Create([FromForm] string title, [FromForm] string content, [FromForm] string? category, [FromForm] string? author)
    {
        if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(content))
        {
            return BadRequest("Title and content are required.");
        }

        _announcementService.Create(new CreateAnnouncementRequest(title, content, category, author));
        return Redirect("/announcements");
    }

    // POST /announcements/{id}/delete -> Delete announcement
    [HttpPost("{id:int}/delete")]
    public IActionResult Delete(int id)
    {
        _announcementService.Delete(id);
        return Redirect("/announcements");
    }

    private string RenderAnnouncementListView(IEnumerable<Announcement> announcements)
    {
        var cards = string.Join("\n", announcements.Select(a => $"""
            <div class="announcement-item">
                <div class="announcement-header">
                    <div>
                        <span class="badge badge-{a.Category.ToLower()}">{a.Category}</span>
                        <h3 style="margin-top:0.4rem;color:#fff;">{a.Title}</h3>
                    </div>
                    <form method="POST" action="/announcements/{a.Id}/delete" onsubmit="return confirm('Delete this announcement?');" style="margin:0;">
                        <button type="submit" class="btn btn-danger">Delete</button>
                    </form>
                </div>
                <p style="color:#cbd5e1;margin:0.75rem 0;">{a.Content}</p>
                <div style="font-size:0.85rem;color:#94a3b8;display:flex;justify-content:space-between;">
                    <span>By: <strong>{a.Author}</strong></span>
                    <span>{a.CreatedAt:yyyy-MM-dd HH:mm} UTC</span>
                </div>
            </div>
        """));

        return $$"""
        <!DOCTYPE html>
        <html lang="en">
        <head>
            <meta charset="UTF-8">
            <meta name="viewport" content="width=device-width, initial-scale=1.0">
            <title>Announcements (View Layer) - Week 04</title>
            <style>
                :root {
                    --bg: #0f172a;
                    --card-bg: #1e293b;
                    --text: #f8fafc;
                    --text-muted: #94a3b8;
                    --primary: #4f46e5;
                    --accent: #10b981;
                    --border: #334155;
                }
                * { box-sizing: border-box; margin: 0; padding: 0; }
                body {
                    font-family: -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, sans-serif;
                    background-color: var(--bg);
                    color: var(--text);
                    line-height: 1.6;
                    padding: 2rem;
                }
                .container { max-width: 900px; margin: 0 auto; }
                header {
                    display: flex;
                    justify-content: space-between;
                    align-items: center;
                    border-bottom: 1px solid var(--border);
                    padding-bottom: 1.5rem;
                    margin-bottom: 2rem;
                }
                h1 { font-size: 1.8rem; font-weight: 700; color: #fff; }
                nav a {
                    color: var(--text-muted);
                    text-decoration: none;
                    margin-left: 1.5rem;
                    font-weight: 500;
                    transition: color 0.2s;
                }
                nav a:hover, nav a.active { color: #fff; }
                .card {
                    background-color: var(--card-bg);
                    border: 1px solid var(--border);
                    border-radius: 12px;
                    padding: 1.5rem;
                    margin-bottom: 2rem;
                }
                .announcement-item {
                    background: #1e293b;
                    border: 1px solid var(--border);
                    border-radius: 10px;
                    padding: 1.25rem;
                    margin-bottom: 1rem;
                }
                .announcement-header {
                    display: flex;
                    justify-content: space-between;
                    align-items: flex-start;
                }
                .badge {
                    display: inline-block;
                    padding: 0.2rem 0.6rem;
                    border-radius: 9999px;
                    font-size: 0.75rem;
                    font-weight: 700;
                    text-transform: uppercase;
                }
                .badge-reunion { background: rgba(79, 70, 229, 0.2); color: #818cf8; }
                .badge-mentorship { background: rgba(16, 185, 129, 0.2); color: #34d399; }
                .badge-career { background: rgba(245, 158, 11, 0.2); color: #fbbf24; }
                .badge-general { background: rgba(148, 163, 184, 0.2); color: #94a3b8; }
                .btn {
                    padding: 0.4rem 0.8rem;
                    border-radius: 6px;
                    font-size: 0.85rem;
                    font-weight: 600;
                    border: none;
                    cursor: pointer;
                }
                .btn-primary { background: var(--primary); color: #fff; }
                .btn-danger { background: rgba(239, 68, 68, 0.2); color: #f87171; border: 1px solid rgba(239, 68, 68, 0.3); }
                .btn-danger:hover { background: #ef4444; color: #fff; }
                .form-group { margin-bottom: 1rem; }
                label { display: block; font-size: 0.85rem; color: var(--text-muted); margin-bottom: 0.3rem; }
                input, textarea, select {
                    width: 100%;
                    padding: 0.55rem 0.75rem;
                    border-radius: 6px;
                    border: 1px solid var(--border);
                    background: #0f172a;
                    color: #fff;
                    font-size: 0.9rem;
                    box-sizing: border-box;
                }
            </style>
        </head>
        <body>
            <div class="container">
                <header>
                    <div>
                        <h1>Alumni Announcements (View Layer)</h1>
                        <p style="color: var(--text-muted); font-size: 0.9rem;">Week 04 Homework — AnnouncementController serving HTML Views</p>
                    </div>
                    <nav>
                        <a href="/">Home</a>
                        <a href="/users">Users (View)</a>
                        <a href="/announcements" class="active">Announcements (View)</a>
                        <a href="/api/announcements">API Announcements (JSON)</a>
                        <a href="/api/swagger" style="color: #34d399; font-weight: 700;">Swagger ⚡</a>
                    </nav>
                </header>

                <div class="card">
                    <h3 style="margin-bottom: 1rem; color: #fff;">📢 Publish New Announcement</h3>
                    <form method="POST" action="/announcements">
                        <div class="form-group">
                            <label>Title</label>
                            <input type="text" name="title" placeholder="e.g. Annual Alumni Summit 2026" required>
                        </div>
                        <div class="form-group">
                            <label>Category</label>
                            <select name="category">
                                <option value="General">General</option>
                                <option value="Career">Career</option>
                                <option value="Mentorship">Mentorship</option>
                                <option value="Reunion">Reunion</option>
                            </select>
                        </div>
                        <div class="form-group">
                            <label>Content</label>
                            <textarea name="content" rows="3" placeholder="Provide event details or message..." required></textarea>
                        </div>
                        <div class="form-group">
                            <label>Author / Office</label>
                            <input type="text" name="author" value="Alumni Coordinator">
                        </div>
                        <button type="submit" class="btn btn-primary" style="padding: 0.6rem 1.4rem;">Publish Announcement</button>
                    </form>
                </div>

                <div>
                    <h2 style="font-size: 1.3rem; margin-bottom: 1rem; color: #fff;">Latest Announcements</h2>
                    {{cards}}
                </div>
            </div>
        </body>
        </html>
        """;
    }
}
