using Alumni.Data;
using Alumni.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Alumni.Controllers;

[Route("admin")]
public class AdminController : Controller
{
    private readonly AlumniDbContext _db;

    public AdminController(AlumniDbContext db)
    {
        _db = db;
    }

    // ─── Shared helpers ────────────────────────────────────────────────────────

    private bool IsAdmin(out string? error)
    {
        var role = HttpContext.Session.GetString("UserRole");
        if (string.IsNullOrEmpty(role)) { error = "auth"; return false; }
        if (role != "admin") { error = "forbidden"; return false; }
        error = null;
        return true;
    }

    private static string NavHtml(string adminName)
    {
        return $"""
            <header class="nav-wrapper">
              <div class="nav-pill-container">
                <a href="/" class="nav-brand">
                  <div class="nav-brand-logo">
                    <img src="/assets/iu-logo.svg" onerror="this.onerror=null;this.src='https://upload.wikimedia.org/wikipedia/commons/1/1e/%C4%B0stanbul_%C3%9Cniversitesi_Logo.svg';" alt="İÜ">
                  </div>
                  <div class="nav-brand-text">
                    <span class="nav-brand-title">İSTANBUL ÜNİVERSİTESİ</span>
                    <span class="nav-brand-sub">Admin Panel · 1453</span>
                  </div>
                </a>
                <nav class="nav-links">
                  <a href="/" class="nav-link">Ana Sayfa</a>
                  <a href="/users" class="nav-link">Mezunlar</a>
                  <a href="/jobs" class="nav-link">Kariyer</a>
                  <a href="/announcements" class="nav-link">Duyurular</a>
                  <a href="/admin" class="nav-link" style="color:var(--green);font-weight:700;">🛡️ Admin Panel</a>
                </nav>
                <div class="nav-actions">
                  <span class="nav-user-badge">🛡️ {System.Net.WebUtility.HtmlEncode(adminName)}</span>
                  <a href="/auth/logout" class="btn btn-ghost" style="font-size:0.82rem;">Çıkış</a>
                </div>
              </div>
            </header>
            """;
    }

    private const string AdminCss = """
        :root {
          --canvas: #FAF9F6; --card: #FFFFFF; --inset: #F5F4EE;
          --hairline: #E2DFD6; --hairline-hover: #C8DBCF;
          --green: #004225; --green-hover: #065A35; --green-tint: #E8F0EB;
          --text: #1C2920; --muted: #55685C; --light: #7E9084;
          --radius-pill: 9999px; --radius-card: 4px;
          --rose: #BE123C; --rose-tint: #FFE4E6;
          --gold: #B45309; --gold-tint: #FEF3C7;
        }
        *, *::before, *::after { box-sizing: border-box; margin: 0; padding: 0; }
        body {
          font-family: 'Inter', -apple-system, BlinkMacSystemFont, sans-serif;
          background-color: var(--canvas);
          background-image: url('/assets/iu-gate-silhouette.svg');
          background-repeat: no-repeat; background-position: center 40px;
          background-size: 1100px auto; background-attachment: fixed;
          color: var(--text); min-height: 100vh;
        }
        .nav-wrapper { position: sticky; top: 1rem; z-index: 100; display: flex; justify-content: center; padding: 0 1rem; margin-bottom: 2rem; }
        .nav-pill-container { display: flex; align-items: center; gap: 1.25rem; background: rgba(250,249,246,0.94); border: 1px solid var(--hairline); border-radius: var(--radius-pill); padding: 0.55rem 1.25rem; backdrop-filter: blur(20px); -webkit-backdrop-filter: blur(20px); max-width: 1000px; width: 100%; }
        .nav-brand { display: flex; align-items: center; gap: 0.65rem; text-decoration: none; flex-shrink: 0; }
        .nav-brand-logo img { width: 32px; height: 32px; object-fit: contain; display: block; }
        .nav-brand-text { display: flex; flex-direction: column; line-height: 1.1; }
        .nav-brand-title { font-size: 0.65rem; font-weight: 700; color: var(--green); letter-spacing: 0.04em; text-transform: uppercase; }
        .nav-brand-sub { font-size: 0.6rem; color: var(--muted); }
        .nav-links { display: flex; align-items: center; gap: 0.1rem; flex: 1; }
        .nav-link { color: var(--muted); text-decoration: none; font-size: 0.85rem; font-weight: 500; padding: 0.4rem 0.75rem; border-radius: var(--radius-pill); transition: color 0.15s, background 0.15s; }
        .nav-link:hover { color: var(--green); background: var(--green-tint); }
        .nav-actions { display: flex; align-items: center; gap: 0.5rem; flex-shrink: 0; }
        .nav-user-badge { font-size: 0.82rem; font-weight: 600; color: var(--green); background: var(--green-tint); border: 1px solid var(--hairline-hover); border-radius: var(--radius-pill); padding: 0.3rem 0.75rem; }
        .btn { display: inline-flex; align-items: center; gap: 0.4rem; padding: 0.5rem 1.1rem; border-radius: var(--radius-pill); font-size: 0.82rem; font-weight: 600; cursor: pointer; text-decoration: none; border: 1px solid transparent; transition: all 0.15s; }
        .btn-primary { background: var(--green); color: #fff; border-color: var(--green); }
        .btn-primary:hover { background: var(--green-hover); }
        .btn-danger { background: var(--rose-tint); color: var(--rose); border-color: #FECDD3; }
        .btn-danger:hover { background: var(--rose); color: #fff; }
        .btn-ghost { background: transparent; color: var(--muted); border-color: var(--hairline); }
        .btn-ghost:hover { background: var(--inset); }
        .btn-warning { background: var(--gold-tint); color: var(--gold); border-color: #FDE68A; }
        .page-container { max-width: 1100px; margin: 0 auto; padding: 0 1.5rem 4rem; }
        .stats-row { display: grid; grid-template-columns: repeat(auto-fit, minmax(160px, 1fr)); gap: 1rem; margin-bottom: 2rem; }
        .stat-card { background: var(--card); border: 1px solid var(--hairline); border-radius: var(--radius-card); padding: 1.25rem 1.5rem; }
        .stat-number { font-size: 2rem; font-weight: 800; color: var(--green); }
        .stat-label { font-size: 0.8rem; color: var(--muted); margin-top: 0.25rem; font-weight: 500; }
        .section-header { display: flex; align-items: center; justify-content: space-between; margin-bottom: 0.85rem; }
        .section-title { font-size: 1rem; font-weight: 700; color: var(--text); }
        .table-card { background: var(--card); border: 1px solid var(--hairline); border-radius: var(--radius-card); overflow: hidden; margin-bottom: 2rem; }
        table { width: 100%; border-collapse: collapse; font-size: 0.875rem; }
        thead { background: var(--inset); }
        th { padding: 0.75rem 1rem; text-align: left; font-size: 0.75rem; font-weight: 700; color: var(--muted); text-transform: uppercase; letter-spacing: 0.04em; border-bottom: 1px solid var(--hairline); }
        td { padding: 0.75rem 1rem; border-bottom: 1px solid var(--hairline-subtle, #EBE8E0); vertical-align: middle; }
        tr:last-child td { border-bottom: none; }
        tr:hover td { background: #FEFEFD; }
        .badge { display: inline-flex; align-items: center; padding: 0.2rem 0.65rem; border-radius: var(--radius-pill); font-size: 0.72rem; font-weight: 700; text-transform: uppercase; letter-spacing: 0.04em; }
        .badge-admin { background: var(--green-tint); color: var(--green); border: 1px solid var(--hairline-hover); }
        .badge-alumni { background: var(--gold-tint); color: var(--gold); border: 1px solid #FDE68A; }
        .badge-student { background: #E0F2FE; color: #0284C7; border: 1px solid #BAE6FD; }
        .badge-faculty { background: #F3E8FF; color: #7C3AED; border: 1px solid #DDD6FE; }
        .alert { padding: 0.75rem 1rem; border-radius: var(--radius-card); margin-bottom: 1.25rem; font-size: 0.88rem; font-weight: 500; }
        .alert-success { background: var(--green-tint); color: var(--green); border: 1px solid var(--hairline-hover); }
        .alert-error { background: var(--rose-tint); color: var(--rose); border: 1px solid #FECDD3; }
        .page-header { margin-bottom: 1.75rem; }
        .page-header h1 { font-size: 1.5rem; font-weight: 800; color: var(--green); }
        .page-header p { font-size: 0.88rem; color: var(--muted); margin-top: 0.3rem; }
        select.role-select { padding: 0.3rem 0.5rem; border: 1px solid var(--hairline); border-radius: var(--radius-pill); font-family: inherit; font-size: 0.82rem; background: var(--canvas); color: var(--text); cursor: pointer; }
    """;

    private string AdminShell(string title, string body)
    {
        var adminName = HttpContext.Session.GetString("UserName") ?? "Admin";
        return $$"""
            <!DOCTYPE html>
            <html lang="tr">
            <head>
              <meta charset="UTF-8">
              <meta name="viewport" content="width=device-width, initial-scale=1.0">
              <title>{{title}} — Admin Panel</title>
              <link rel="preconnect" href="https://fonts.googleapis.com">
              <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
              <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700;800&display=swap" rel="stylesheet">
              <style>
                {{AdminCss}}
              </style>
            </head>
            <body>
              {{NavHtml(adminName)}}
              {{body}}
            </body>
            </html>
            """;
    }

    // ─── GET /admin ────────────────────────────────────────────────────────────
    [HttpGet("")]
    public async Task<IActionResult> Index([FromQuery] string? msg)
    {
        if (!IsAdmin(out var err))
            return err == "auth" ? Redirect("/auth/login?returnUrl=/admin") : Redirect("/?error=forbidden");

        var users = await _db.Users.OrderBy(u => u.Id).ToListAsync();
        var announcements = await _db.Announcements.OrderByDescending(a => a.CreatedAt).Take(10).ToListAsync();
        var jobs = await _db.JobPostings.OrderByDescending(j => j.CreatedAt).Take(10).ToListAsync();

        var totalUsers = users.Count;
        var adminCount = users.Count(u => u.Role == "admin");
        var alumniCount = users.Count(u => u.Role == "alumni");

        var alertHtml = msg != null
            ? $"""<div class="alert alert-success">✓ {System.Net.WebUtility.HtmlEncode(msg)}</div>"""
            : "";

        var userRows = string.Join("\n", users.Select(u =>
        {
            var badgeCss = u.Role switch { "admin" => "badge-admin", "student" => "badge-student", "faculty" => "badge-faculty", _ => "badge-alumni" };
            return $"""
                <tr>
                  <td style="color:var(--muted);font-size:0.78rem;">#{u.Id}</td>
                  <td style="font-weight:600;">{System.Net.WebUtility.HtmlEncode(u.Name)}</td>
                  <td style="color:var(--muted);">{System.Net.WebUtility.HtmlEncode(u.Email)}</td>
                  <td>{System.Net.WebUtility.HtmlEncode(u.Department)}</td>
                  <td>{u.GraduationYear}</td>
                  <td>
                    <form method="POST" action="/admin/users/{u.Id}/role" style="display:inline;">
                      <select name="role" class="role-select" onchange="this.form.submit()">
                        <option value="alumni" {(u.Role=="alumni"?"selected":"")}>alumni</option>
                        <option value="admin" {(u.Role=="admin"?"selected":"")}>admin</option>
                        <option value="student" {(u.Role=="student"?"selected":"")}>student</option>
                        <option value="faculty" {(u.Role=="faculty"?"selected":"")}>faculty</option>
                      </select>
                    </form>
                  </td>
                  <td style="font-size:0.78rem;color:var(--light);">{u.CreatedAt:dd.MM.yyyy}</td>
                  <td>
                    <form method="POST" action="/admin/users/{u.Id}/delete" onsubmit="return confirm('{System.Net.WebUtility.HtmlEncode(u.Name)} silinsin mi?');" style="display:inline;">
                      <button type="submit" class="btn btn-danger" style="padding:0.25rem 0.65rem;font-size:0.75rem;">Sil</button>
                    </form>
                  </td>
                </tr>
                """;
        }));

        var annRows = string.Join("\n", announcements.Select(a => $"""
            <tr>
              <td style="font-weight:600;">{System.Net.WebUtility.HtmlEncode(a.Title)}</td>
              <td><span class="badge badge-alumni">{a.Category}</span></td>
              <td style="color:var(--muted);">{a.Author}</td>
              <td style="font-size:0.78rem;color:var(--light);">{a.CreatedAt:dd.MM.yyyy}</td>
              <td>
                <form method="POST" action="/announcements/{a.Id}/delete" onsubmit="return confirm('Silinsin mi?');" style="display:inline;">
                  <button type="submit" class="btn btn-danger" style="padding:0.25rem 0.65rem;font-size:0.75rem;">Sil</button>
                </form>
              </td>
            </tr>
            """));

        var body = $"""
            <div class="page-container">
              <div class="page-header">
                <h1>🛡️ Admin Paneli</h1>
                <p>Sistem verilerini yönetin · İstanbul Üniversitesi Mezun Takip Sistemi</p>
              </div>
              {alertHtml}

              <div class="stats-row">
                <div class="stat-card">
                  <div class="stat-number">{totalUsers}</div>
                  <div class="stat-label">Toplam Kullanıcı</div>
                </div>
                <div class="stat-card">
                  <div class="stat-number">{alumniCount}</div>
                  <div class="stat-label">Mezun</div>
                </div>
                <div class="stat-card">
                  <div class="stat-number">{adminCount}</div>
                  <div class="stat-label">Admin</div>
                </div>
                <div class="stat-card">
                  <div class="stat-number">{announcements.Count}</div>
                  <div class="stat-label">Duyuru</div>
                </div>
                <div class="stat-card">
                  <div class="stat-number">{jobs.Count}</div>
                  <div class="stat-label">İş İlanı</div>
                </div>
              </div>

              <!-- Users Table -->
              <div class="section-header">
                <span class="section-title">👥 Kullanıcı Yönetimi</span>
              </div>
              <div class="table-card">
                <table>
                  <thead>
                    <tr>
                      <th>ID</th><th>Ad Soyad</th><th>E-posta</th><th>Bölüm</th>
                      <th>Mezun Yılı</th><th>Rol (Değiştir)</th><th>Tarih</th><th>İşlem</th>
                    </tr>
                  </thead>
                  <tbody>{userRows}</tbody>
                </table>
              </div>

              <!-- Announcements Table -->
              <div class="section-header">
                <span class="section-title">📢 Duyurular</span>
                <a href="/announcements" class="btn btn-ghost">Tümünü Gör</a>
              </div>
              <div class="table-card">
                <table>
                  <thead>
                    <tr><th>Başlık</th><th>Kategori</th><th>Yazar</th><th>Tarih</th><th>İşlem</th></tr>
                  </thead>
                  <tbody>{annRows}</tbody>
                </table>
              </div>
            </div>
            """;

        return Content(AdminShell("Admin Panel", body), "text/html");
    }

    // ─── POST /admin/users/{id}/role ───────────────────────────────────────────
    [HttpPost("users/{id:int}/role")]
    public async Task<IActionResult> ChangeRole(int id, [FromForm] string role)
    {
        if (!IsAdmin(out var err))
            return err == "auth" ? Redirect("/auth/login") : Redirect("/");

        var validRoles = new[] { "alumni", "admin", "student", "faculty" };
        if (!validRoles.Contains(role)) return Redirect("/admin");

        var user = await _db.Users.FindAsync(id);
        if (user != null)
        {
            user.Role = role;
            user.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
        }

        return Redirect("/admin?msg=Kullanıcı+rolü+güncellendi.");
    }

    // ─── POST /admin/users/{id}/delete ────────────────────────────────────────
    [HttpPost("users/{id:int}/delete")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        if (!IsAdmin(out var err))
            return err == "auth" ? Redirect("/auth/login") : Redirect("/");

        var currentUserId = HttpContext.Session.GetString("UserId");
        if (currentUserId == id.ToString())
            return Redirect("/admin?msg=Kendi+hesabınızı+silemezsiniz.");

        var user = await _db.Users.FindAsync(id);
        if (user != null)
        {
            _db.Users.Remove(user);
            await _db.SaveChangesAsync();
        }

        return Redirect("/admin?msg=Kullanıcı+silindi.");
    }
}
