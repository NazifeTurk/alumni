using Alumni.Models;
using Alumni.Services;
using Microsoft.AspNetCore.Mvc;

namespace Alumni.Controllers;

[Route("jobs")]
public class JobPostingController : Controller
{
    private readonly IJobPostingService _jobService;
    private readonly IUserService _userService;

    public JobPostingController(IJobPostingService jobService, IUserService userService)
    {
        _jobService = jobService;
        _userService = userService;
    }

    // GET /jobs -> Listing all jobs HTML View
    [HttpGet("")]
    public IActionResult Index()
    {
        var jobs = _jobService.GetAll();
        return Content(RenderJobListView(jobs), "text/html");
    }

    // GET /jobs/create -> Create Job Form
    [HttpGet("create")]
    public IActionResult CreateForm()
    {
        var users = _userService.GetAll();
        return Content(RenderCreateJobForm(users), "text/html");
    }

    // POST /jobs -> Create job from HTML Form
    [HttpPost("")]
    public IActionResult Create([FromForm] int userId, [FromForm] string title, [FromForm] string company, [FromForm] string? location, [FromForm] string? description)
    {
        if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(company))
        {
            return BadRequest("Title and Company are required.");
        }

        _jobService.CreateForUser(userId, new CreateJobPostingRequest(title, company, location, description));
        return Redirect("/jobs");
    }

    // GET /jobs/{id} -> Single job detail view with applications
    [HttpGet("{id:int}")]
    public IActionResult Details(int id)
    {
        var job = _jobService.GetById(id);
        if (job == null)
        {
            return NotFound("Job posting not found.");
        }

        var applications = _jobService.GetApplicationsForJob(id);
        var allUsers = _userService.GetAll();
        return Content(RenderJobDetailView(job, applications, allUsers), "text/html");
    }

    // POST /jobs/{id}/apply -> Apply to job from HTML Form
    [HttpPost("{id:int}/apply")]
    public IActionResult Apply(int id, [FromForm] int userId)
    {
        var (app, error, statusCode) = _jobService.Apply(id, userId);
        if (statusCode == 409)
        {
            // Already applied
            return Content($"<script>alert('{error}'); window.location.href='/jobs/{id}';</script>", "text/html");
        }
        if (statusCode == 404)
        {
            return NotFound(error);
        }

        return Redirect($"/jobs/{id}");
    }

    // --- HTML View Renderers ---
    private string RenderJobListView(IEnumerable<JobPosting> jobs)
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
               <a href="/auth/logout" class="btn btn-secondary" style="padding:0.25rem 0.65rem;font-size:0.75rem;">Çıkış</a>
               """
            : """
              <a href="/auth/login">Giriş</a>
              <a href="/auth/register" class="btn btn-primary" style="padding:0.25rem 0.65rem;font-size:0.75rem;">Kayıt Ol</a>
              """;

        var cards = string.Join("\n", jobs.Select(j => $"""
            <div class="job-card">
                <div style="display: flex; justify-content: space-between; align-items: flex-start; gap: 1rem;">
                    <div>
                        <div class="company-badge">{j.Company}</div>
                        <h3 style="font-size: 1.3rem; margin: 0.4rem 0 0.2rem 0; color: #fff;">{j.Title}</h3>
                        <p style="color: #94a3b8; font-size: 0.9rem;">📍 {j.Location} &bull; Posted by: <strong>{(j.User != null ? j.User.Name : $"User #{j.UserId}")}</strong></p>
                    </div>
                    <span class="badge-status {(j.IsActive ? "badge-active" : "badge-closed")}">
                        {(j.IsActive ? "Active Opening" : "Closed")}
                    </span>
                </div>
                <p style="color: #cbd5e1; margin: 1rem 0; font-size: 0.95rem; line-height: 1.5;">{j.Description}</p>
                <div style="display: flex; gap: 0.75rem; align-items: center; border-top: 1px solid #334155; padding-top: 1rem; margin-top: 1rem;">
                    <a href="/jobs/{j.Id}" class="btn btn-primary" style="padding: 0.4rem 0.9rem; font-size: 0.9rem;">View Details &amp; Apply &rarr;</a>
                    <span style="color: #64748b; font-size: 0.85rem;">📅 {j.CreatedAt:MMM dd, yyyy}</span>
                </div>
            </div>
        """));

        return $$"""
        <!DOCTYPE html>
        <html lang="tr">
        <head>
            <meta charset="UTF-8">
            <meta name="viewport" content="width=device-width, initial-scale=1.0">
            <title>İş ve Staj İlanları — İstanbul Üniversitesi Mezun Takip</title>
            <link rel="preconnect" href="https://fonts.googleapis.com">
            <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
            <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700;800&display=swap" rel="stylesheet">
            <style>
                :root {
                    --bg: #FAF9F6;
                    --card-bg: #FFFFFF;
                    --border: #E2DFD6;
                    --text: #1C2920;
                    --text-muted: #55685C;
                    --primary: #004225;
                    --primary-hover: #065A35;
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
                .container { max-width: 960px; margin: 0 auto; }
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
                nav { display: flex; align-items: center; gap: 1rem; flex-wrap: wrap; }
                nav a {
                    color: var(--text-muted);
                    text-decoration: none;
                    font-weight: 600;
                    font-size: 0.9rem;
                    transition: color 0.15s;
                }
                nav a:hover, nav a.active { color: var(--primary); }
                .btn {
                    display: inline-flex;
                    align-items: center;
                    padding: 0.5rem 1.2rem;
                    border-radius: 9999px;
                    font-weight: 700;
                    text-decoration: none;
                    cursor: pointer;
                    border: 1px solid transparent;
                    font-size: 0.88rem;
                    transition: background 0.15s;
                }
                .btn-primary { background: var(--primary); color: #fff; }
                .btn-primary:hover { background: var(--primary-hover); }
                .job-card {
                    background: var(--card-bg);
                    border: 1px solid var(--border);
                    border-radius: 4px;
                    padding: 1.75rem;
                    margin-bottom: 1.5rem;
                    box-shadow: none;
                }
                .company-badge {
                    display: inline-block;
                    background: var(--pill-bg);
                    color: var(--primary);
                    border: 1px solid var(--pill-border);
                    padding: 0.2rem 0.6rem;
                    border-radius: 9999px;
                    font-size: 0.78rem;
                    font-weight: 700;
                    text-transform: uppercase;
                }
                .badge-status {
                    display: inline-block;
                    padding: 0.25rem 0.75rem;
                    border-radius: 9999px;
                    font-size: 0.78rem;
                    font-weight: 700;
                }
                .badge-active { background: #D1FAE5; color: #065F46; border: 1px solid #A7F3D0; }
                .badge-closed { background: #FFE4E6; color: #9F1239; border: 1px solid #FECDD3; }
            </style>
        </head>
        <body>
            <div class="container">
                <header>
                    <div class="header-brand">
                        <img src="/assets/iu-logo.svg" onerror="this.onerror=null; this.src='https://upload.wikimedia.org/wikipedia/commons/1/1e/%C4%B0stanbul_%C3%9Cniversitesi_Logo.svg';" alt="IU Logo">
                        <div>
                            <h1>Kariyer &amp; İlan Havuzu</h1>
                            <p style="color: var(--text-muted); font-size: 0.85rem;">İstanbul Üniversitesi Mezunlar Ağı Açık Pozisyonları</p>
                        </div>
                    </div>
                    <div style="display: flex; align-items: center; gap: 1rem;">
                        <a href="/jobs/create" class="btn btn-primary">+ Yeni İlan Ekle</a>
                        <nav>
                            <a href="/">Ana Sayfa</a>
                            <a href="/users">Mezunlar</a>
                            <a href="/announcements">Duyurular</a>
                            <a href="/jobs" class="active">İlanlar</a>
                            {profileLink}
                            {adminLink}
                            {authActions}
                        </nav>
                    </div>
                </header>

                <div class="jobs-list">
                    {{cards}}
                </div>
            </div>
        </body>
        </html>
        """;
    }

    private string RenderCreateJobForm(IEnumerable<User> users)
    {
        var userOptions = string.Join("\n", users.Select(u => $"<option value=\"{u.Id}\">{u.Name} (#{u.Id} - {u.Email})</option>"));

        return $$"""
        <!DOCTYPE html>
        <html lang="tr">
        <head>
            <meta charset="UTF-8">
            <title>Yeni İlan Paylaş — İstanbul Üniversitesi</title>
            <style>
                body { font-family: sans-serif; background: #FAF9F6; color: #1C2920; padding: 2rem 1rem; }
                .container { max-width: 650px; margin: 0 auto; background: #FFFFFF; padding: 2.5rem; border-radius: 4px; border: 1px solid #E2DFD6; box-shadow: none; }
                .logo { display: flex; align-items: center; gap: 0.75rem; margin-bottom: 1.5rem; }
                .logo img { width: 44px; height: 44px; }
                .form-group { margin-bottom: 1.25rem; }
                label { display: block; margin-bottom: 0.4rem; color: #55685C; font-weight: 700; font-size: 0.85rem; text-transform: uppercase; }
                input, select, textarea { width: 100%; padding: 0.65rem 0.75rem; background: #FFFFFF; border: 1px solid #E2DFD6; border-radius: 4px; color: #1C2920; font-size: 0.95rem; box-sizing: border-box; }
                input:focus, select:focus, textarea:focus { outline: none; border-color: #004225; }
                button { background: #004225; color: #fff; padding: 0.7rem 1.6rem; border: none; border-radius: 9999px; font-weight: 700; cursor: pointer; }
                button:hover { background: #065A35; }
                a { color: #55685C; text-decoration: none; margin-left: 1rem; font-size: 0.9rem; }
            </style>
        </head>
        <body>
            <div class="container">
                <div class="logo">
                    <img src="/assets/iu-logo.svg" onerror="this.src='https://upload.wikimedia.org/wikipedia/commons/1/1e/%C4%B0stanbul_%C3%9Cniversitesi_Logo.svg';" alt="IU">
                    <div>
                        <h2 style="color: #004225; font-size: 1.4rem;">Yeni Kariyer İlanı Yayınla</h2>
                        <span style="color: #55685C; font-size: 0.82rem;">İstanbul Üniversitesi Mezun Takip Sistemi</span>
                    </div>
                </div>
                <form method="POST" action="/jobs">
                    <div class="form-group">
                        <label for="userId">İlanı Veren Mezun (Yazar):</label>
                        <select name="userId" id="userId" required>
                            {{userOptions}}
                        </select>
                    </div>
                    <div class="form-group">
                        <label for="title">Pozisyon Başlığı (Title):</label>
                        <input type="text" name="title" id="title" placeholder="Örn: Kıdemli Bulut Mimarı" required />
                    </div>
                    <div class="form-group">
                        <label for="company">Şirket / Kurum (Company):</label>
                        <input type="text" name="company" id="company" placeholder="Örn: Trendyol Teknoloji" required />
                    </div>
                    <div class="form-group">
                        <label for="location">Konum (Location):</label>
                        <input type="text" name="location" id="location" value="İstanbul / Hibrit" />
                    </div>
                    <div class="form-group">
                        <label for="description">Açıklama &amp; Nitelikler:</label>
                        <textarea name="description" id="description" rows="4" placeholder="İş tanımı, aranan teknolojiler ve başvuru koşulları..." required></textarea>
                    </div>
                    <button type="submit">İlanı Yayınla</button>
                    <a href="/jobs">İptal</a>
                </form>
            </div>
        </body>
        </html>
        """;
    }

    private string RenderJobDetailView(JobPosting job, IEnumerable<Application> applications, IEnumerable<User> allUsers)
    {
        var appRows = applications.Any()
            ? string.Join("\n", applications.Select(a => $"""
                <tr style="border-bottom: 1px solid #EBE8E0;">
                    <td style="padding: 0.75rem 1rem;"><strong>#{a.UserId}</strong> {(a.User != null ? a.User.Name : "Aday")}</td>
                    <td style="padding: 0.75rem 1rem;">{(a.User != null ? a.User.Email : "")}</td>
                    <td style="padding: 0.75rem 1rem;"><span style="background: #E8F0EB; color: #004225; padding: 0.2rem 0.6rem; border-radius: 9999px; font-size: 0.78rem; font-weight: 700; border: 1px solid #C8DBCF;">{a.Status}</span></td>
                    <td style="padding: 0.75rem 1rem; color: #55685C; font-size: 0.85rem;">{a.CreatedAt:yyyy-MM-dd HH:mm}</td>
                </tr>
            """))
            : "<tr><td colspan=\"4\" style=\"padding: 1.5rem; color: #55685C; text-align: center;\">Henüz başvuru yapılmamış. İlk başvuran siz olun!</td></tr>";

        var userSelect = string.Join("\n", allUsers.Select(u => $"<option value=\"{u.Id}\">{u.Name} (#{u.Id})</option>"));

        return $$"""
        <!DOCTYPE html>
        <html lang="tr">
        <head>
            <meta charset="UTF-8">
            <title>{{job.Title}} — {{job.Company}}</title>
            <style>
                body { font-family: sans-serif; background: #FAF9F6; color: #1C2920; padding: 2rem 1rem; }
                .container { max-width: 850px; margin: 0 auto; }
                .card { background: #FFFFFF; border: 1px solid #E2DFD6; border-radius: 4px; padding: 2.25rem; margin-bottom: 2rem; box-shadow: none; }
                .btn { display: inline-flex; align-items: center; padding: 0.55rem 1.25rem; border-radius: 9999px; font-weight: 700; text-decoration: none; cursor: pointer; border: none; font-size: 0.88rem; }
                .btn-primary { background: #004225; color: #fff; }
                .btn-primary:hover { background: #065A35; }
                a { color: #004225; text-decoration: none; font-weight: 600; }
                a:hover { text-decoration: underline; }
                table { width: 100%; border-collapse: collapse; margin-top: 1rem; }
                th { text-align: left; padding: 0.75rem 1rem; color: #55685C; border-bottom: 1px solid #E2DFD6; background: #F5F4EE; font-size: 0.8rem; text-transform: uppercase; }
            </style>
        </head>
        <body>
            <div class="container">
                <p style="margin-bottom: 1.25rem;"><a href="/jobs">&larr; Tüm İlanlara Dön</a></p>
                <div class="card">
                    <span style="background: #E8F0EB; color: #004225; padding: 0.2rem 0.7rem; border-radius: 9999px; font-weight: 700; font-size: 0.8rem; border: 1px solid #C8DBCF;">{{job.Company}}</span>
                    <h1 style="color: #004225; margin: 0.6rem 0 0.25rem 0; font-size: 1.85rem;">{{job.Title}}</h1>
                    <p style="color: #55685C; font-size: 0.95rem; margin-bottom: 1.75rem;">📍 {{job.Location}} &bull; İlanı Veren: <strong>{{(job.User != null ? job.User.Name : $"User #{job.UserId}")}}</strong> &bull; Tarih: {{job.CreatedAt:yyyy-MM-dd}}</p>
                    
                    <h3 style="color: #0F2A1D; margin-bottom: 0.5rem; font-size: 1.1rem;">İlan Açıklaması:</h3>
                    <p style="color: #1C2920; line-height: 1.6; margin-bottom: 2rem;">{{job.Description}}</p>

                    <!-- Quick Apply Box -->
                    <div style="background: #F5F4EE; border: 1px solid #E2DFD6; border-radius: 4px; padding: 1.25rem;">
                        <h4 style="color: #004225; margin-bottom: 0.75rem;">🚀 Bu İlana Başvur:</h4>
                        <form method="POST" action="/jobs/{{job.Id}}/apply" style="display: flex; gap: 0.75rem; align-items: center; flex-wrap: wrap;">
                            <label for="applicantUserId" style="color: #55685C; font-size: 0.9rem; font-weight: 600;">Başvuran Mezun:</label>
                            <select name="userId" id="applicantUserId" style="padding: 0.55rem 0.75rem; background: #FFFFFF; color: #1C2920; border: 1px solid #E2DFD6; border-radius: 4px;">
                                {{userSelect}}
                            </select>
                            <button type="submit" class="btn btn-primary">Başvuruyu Gönder</button>
                        </form>
                    </div>
                </div>

                <div class="card">
                    <h3 style="color: #004225; margin-bottom: 0.5rem;">📋 Gelen Başvurular ({{applications.Count()}}):</h3>
                    <table>
                        <thead>
                            <tr>
                                <th>Aday</th>
                                <th>E-posta</th>
                                <th>Durum</th>
                                <th>Başvuru Tarihi</th>
                            </tr>
                        </thead>
                        <tbody>
                            {{appRows}}
                        </tbody>
                    </table>
                </div>
            </div>
        </body>
        </html>
        """;
    }
}
