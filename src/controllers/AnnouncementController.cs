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
        var userName = HttpContext?.Session?.GetString("UserName");
        var userRole = HttpContext?.Session?.GetString("UserRole") ?? "";
        bool loggedIn = !string.IsNullOrEmpty(userName);

        var adminLink = userRole == "admin"
            ? """<a href="/admin" style="color:var(--primary);font-weight:700;">🛡️ Admin</a>"""
            : "";
        var profileLink = loggedIn
            ? """<a href="/profile">Profilim</a>"""
            : "";
        var authActions = loggedIn
            ? $"""
               <a href="/profile" style="color:var(--primary);font-weight:600;">👤 {System.Net.WebUtility.HtmlEncode(userName)}</a>
               <a href="/auth/logout" class="btn btn-secondary" style="padding:0.25rem 0.65rem;font-size:0.75rem;border:1px solid var(--border);border-radius:9999px;background:#fff;color:var(--text);text-decoration:none;">Çıkış</a>
               """
            : """
              <a href="/auth/login">Giriş</a>
              <a href="/auth/register" class="btn btn-primary" style="padding:0.25rem 0.65rem;font-size:0.75rem;border-radius:9999px;text-decoration:none;">Kayıt Ol</a>
              """;

        var cards = string.Join("\n", announcements.Select(a => $"""
            <div class="announcement-item">
                <div class="announcement-header">
                    <div>
                        <span class="badge badge-{a.Category.ToLower()}">{a.Category}</span>
                        <h3 style="margin-top:0.4rem;color:#0F2A1D;font-size:1.15rem;">{a.Title}</h3>
                    </div>
                    <form method="POST" action="/announcements/{a.Id}/delete" onsubmit="return confirm('Bu duyuruyu silmek istediğinize emin misiniz?');" style="margin:0;">
                        <button type="submit" class="btn btn-danger">Sil</button>
                    </form>
                </div>
                <p style="color:#2D4235;margin:0.75rem 0;line-height:1.6;">{a.Content}</p>
                <div style="font-size:0.82rem;color:#55685C;display:flex;justify-content:space-between;border-top:1px solid #EBE8E0;padding-top:0.6rem;">
                    <span>Yayınlayan: <strong>{a.Author}</strong></span>
                    <span>{a.CreatedAt:yyyy-MM-dd HH:mm} UTC</span>
                </div>
            </div>
        """));

        return $$"""
        <!DOCTYPE html>
        <html lang="tr">
        <head>
            <meta charset="UTF-8">
            <meta name="viewport" content="width=device-width, initial-scale=1.0">
            <title>Duyurular — İstanbul Üniversitesi Mezun Takip</title>
            <link rel="preconnect" href="https://fonts.googleapis.com">
            <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
            <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700;800&display=swap" rel="stylesheet">
            <style>
                :root {
                    --bg: #FAF9F6;
                    --card-bg: #FFFFFF;
                    --text: #1C2920;
                    --text-muted: #55685C;
                    --primary: #004225;
                    --primary-hover: #065A35;
                    --accent: #059669;
                    --border: #E2DFD6;
                    --pill-bg: #E8F0EB;
                    --pill-border: #C8DBCF;
                }
                * { box-sizing: border-box; margin: 0; padding: 0; }
                body {
                    font-family: 'Inter', sans-serif;
                    background-color: var(--bg);
                    background-image: url('/assets/iu-gate-silhouette.svg');
                    background-repeat: no-repeat;
                    background-position: center 30px;
                    background-size: 1100px auto;
                    background-attachment: fixed;
                    color: var(--text);
                    line-height: 1.6;
                    padding: 2rem 1rem;
                    -webkit-font-smoothing: antialiased;
                }
                .container { max-width: 900px; margin: 0 auto; }
                header {
                    display: flex;
                    justify-content: space-between;
                    align-items: center;
                    border-bottom: 1px solid var(--border);
                    padding-bottom: 1.5rem;
                    margin-bottom: 2rem;
                    flex-wrap: wrap;
                    gap: 1rem;
                }
                .header-brand { display: flex; align-items: center; gap: 0.85rem; }
                .header-brand img { width: 44px; height: 44px; object-fit: contain; }
                h1 { font-family: 'Inter', sans-serif; font-size: 1.9rem; font-weight: 400; color: var(--primary); letter-spacing: -0.02em; }
                nav { display: flex; align-items: center; gap: 1.25rem; flex-wrap: wrap; }
                nav a {
                    color: var(--text-muted);
                    text-decoration: none;
                    font-weight: 600;
                    font-size: 0.9rem;
                    transition: color 0.15s;
                }
                nav a:hover, nav a.active { color: var(--primary); }
                .card {
                    background-color: var(--card-bg);
                    border: 1px solid var(--border);
                    border-radius: 4px;
                    padding: 1.75rem;
                    margin-bottom: 2rem;
                    box-shadow: none;
                }
                .announcement-item {
                    background: #FFFFFF;
                    border: 1px solid var(--border);
                    border-radius: 4px;
                    padding: 1.25rem;
                    margin-bottom: 1rem;
                    box-shadow: none;
                }
                .announcement-header {
                    display: flex;
                    justify-content: space-between;
                    align-items: flex-start;
                }
                .badge {
                    display: inline-block;
                    padding: 0.2rem 0.65rem;
                    border-radius: 9999px;
                    font-size: 0.75rem;
                    font-weight: 700;
                    text-transform: uppercase;
                    letter-spacing: 0.04em;
                }
                .badge-reunion { background: #FEF3C7; color: #92400E; border: 1px solid #FDE68A; }
                .badge-mentorship { background: #E8F0EB; color: #004225; border: 1px solid #C8DBCF; }
                .badge-career { background: #E0F2FE; color: #0369A1; border: 1px solid #BAE6FD; }
                .badge-general { background: #F5F4EE; color: #55685C; border: 1px solid #E2DFD6; }
                .btn {
                    padding: 0.45rem 1rem;
                    border-radius: 9999px;
                    font-size: 0.82rem;
                    font-weight: 700;
                    border: 1px solid transparent;
                    cursor: pointer;
                    display: inline-flex;
                    align-items: center;
                }
                .btn-primary { background: var(--primary); color: #fff; }
                .btn-primary:hover { background: var(--primary-hover); }
                .btn-danger { background: #FFE4E6; color: #BE123C; border: 1px solid #FECDD3; }
                .btn-danger:hover { background: #F43F5E; color: #fff; }
                .form-group { margin-bottom: 1.25rem; }
                label { display: block; font-size: 0.82rem; font-weight: 700; color: var(--text-muted); margin-bottom: 0.35rem; text-transform: uppercase; }
                input, textarea, select {
                    width: 100%;
                    padding: 0.65rem 0.75rem;
                    border-radius: 4px;
                    border: 1px solid var(--border);
                    background: #FFFFFF;
                    color: var(--text);
                    font-size: 0.95rem;
                    box-sizing: border-box;
                }
                input:focus, textarea:focus, select:focus { outline: none; border-color: var(--primary); }
            </style>
        </head>
        <body>
            <div class="container">
                <header>
                    <div class="header-brand">
                        <img src="/assets/iu-logo.svg" onerror="this.onerror=null; this.src='https://upload.wikimedia.org/wikipedia/commons/1/1e/%C4%B0stanbul_%C3%9Cniversitesi_Logo.svg';" alt="IU Logo">
                        <div>
                            <h1>Akademik Duyurular</h1>
                            <p style="color: var(--text-muted); font-size: 0.85rem;">İstanbul Üniversitesi &bull; Nazife Türk &bull; Mezun İletişim Portalı</p>
                        </div>
                    </div>
                    <nav>
                        <a href="/">Ana Sayfa</a>
                        <a href="/users">Mezunlar</a>
                        <a href="/jobs">İş İlanları</a>
                        <a href="/announcements" class="active">Duyurular</a>
                        {profileLink}
                        {adminLink}
                        <a href="/swagger" style="color: var(--primary); font-weight: 700;">Swagger API</a>
                        {authActions}
                    </nav>
                </header>

                <div class="card">
                    <h3 style="margin-bottom: 1.25rem; color: var(--primary); font-size: 1.15rem;">📢 Yeni Mezun Duyurusu Yayınla</h3>
                    <form method="POST" action="/announcements">
                        <div class="form-group">
                            <label>Duyuru Başlığı</label>
                            <input type="text" name="title" placeholder="Örn: 2026 Mezunlar Homecoming Buluşması" required>
                        </div>
                        <div class="form-group">
                            <label>Kategori</label>
                            <select name="category">
                                <option value="General">Genel</option>
                                <option value="Career">Kariyer</option>
                                <option value="Mentorship">Mentorluk</option>
                                <option value="Reunion">Homecoming &bull; Buluşma</option>
                            </select>
                        </div>
                        <div class="form-group">
                            <label>İçerik &amp; Detaylar</label>
                            <textarea name="content" rows="3" placeholder="Etkinlik ayrıntıları, yer ve tarih bilgisi..." required></textarea>
                        </div>
                        <div class="form-group">
                            <label>Yayınlayan Birim / Yetkili</label>
                            <input type="text" name="author" value="Mezun Koordinatörlüğü">
                        </div>
                        <button type="submit" class="btn btn-primary" style="padding: 0.65rem 1.6rem;">Duyuruyu Yayınla</button>
                    </form>
                </div>

                <div>
                    <h2 style="font-size: 1.3rem; margin-bottom: 1rem; color: var(--primary); font-family: 'Inter', sans-serif;">Güncel Mezun Duyuruları</h2>
                    {{cards}}
                </div>
            </div>
        </body>
        </html>
        """;
    }
}
