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
            <style>
                :root {
                    --bg: #0f172a;
                    --card-bg: #1e293b;
                    --border: #334155;
                    --text: #f8fafc;
                    --text-muted: #94a3b8;
                    --primary: #4f46e5;
                    --primary-hover: #4338ca;
                    --accent: #10b981;
                }
                * { box-sizing: border-box; margin: 0; padding: 0; }
                body {
                    font-family: -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, sans-serif;
                    background-color: var(--bg);
                    color: var(--text);
                    line-height: 1.6;
                    padding: 2rem 1rem;
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
                nav a {
                    color: var(--text-muted);
                    text-decoration: none;
                    margin-left: 1.25rem;
                    font-weight: 500;
                    transition: color 0.2s;
                }
                nav a:hover, nav a.active { color: #fff; }
                .btn {
                    display: inline-block;
                    padding: 0.6rem 1.2rem;
                    border-radius: 6px;
                    font-weight: 600;
                    text-decoration: none;
                    cursor: pointer;
                    border: none;
                    transition: background 0.2s;
                }
                .btn-primary { background: var(--primary); color: #fff; }
                .btn-primary:hover { background: var(--primary-hover); }
                .job-card {
                    background: var(--card-bg);
                    border: 1px solid var(--border);
                    border-radius: 12px;
                    padding: 1.5rem;
                    margin-bottom: 1.5rem;
                }
                .company-badge {
                    display: inline-block;
                    background: rgba(56, 189, 248, 0.15);
                    color: #38bdf8;
                    border: 1px solid rgba(56, 189, 248, 0.3);
                    padding: 0.2rem 0.6rem;
                    border-radius: 4px;
                    font-size: 0.8rem;
                    font-weight: 700;
                }
                .badge-status {
                    display: inline-block;
                    padding: 0.25rem 0.7rem;
                    border-radius: 9999px;
                    font-size: 0.8rem;
                    font-weight: 600;
                }
                .badge-active { background: rgba(16, 185, 129, 0.15); color: #34d399; border: 1px solid rgba(16, 185, 129, 0.3); }
                .badge-closed { background: rgba(239, 68, 68, 0.15); color: #f87171; border: 1px solid rgba(239, 68, 68, 0.3); }
            </style>
        </head>
        <body>
            <div class="container">
                <header>
                    <div>
                        <h1 style="font-size: 1.7rem; color: #fff;">💼 İş ve Staj İlanları</h1>
                        <p style="color: var(--text-muted); font-size: 0.9rem;">Mezunlarımızın çalıştığı şirketlerdeki açık pozisyonlar ve referral fırsatları</p>
                    </div>
                    <div>
                        <a href="/jobs/create" class="btn btn-primary">+ Yeni İlan Ekle</a>
                        <nav style="display: inline-block; margin-left: 1rem;">
                            <a href="/">Ana Sayfa</a>
                            <a href="/users">Mezunlar</a>
                            <a href="/announcements">Duyurular</a>
                            <a href="/jobs" class="active">İlanlar</a>
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
                body { font-family: -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, sans-serif; background: #0f172a; color: #f8fafc; padding: 2rem; }
                .container { max-width: 650px; margin: 0 auto; background: #1e293b; padding: 2.5rem; border-radius: 12px; border: 1px solid #334155; }
                .form-group { margin-bottom: 1.25rem; }
                label { display: block; margin-bottom: 0.5rem; color: #cbd5e1; font-weight: 600; font-size: 0.95rem; }
                input, select, textarea { width: 100%; padding: 0.75rem; background: #0f172a; border: 1px solid #334155; border-radius: 6px; color: #fff; font-size: 1rem; box-sizing: border-box; }
                button { background: #4f46e5; color: #fff; padding: 0.8rem 1.5rem; border: none; border-radius: 6px; font-weight: 600; cursor: pointer; }
                button:hover { background: #4338ca; }
                a { color: #94a3b8; text-decoration: none; margin-left: 1rem; }
            </style>
        </head>
        <body>
            <div class="container">
                <h2 style="margin-bottom: 1.5rem; color: #fff;">💼 Yeni İlan Paylaş</h2>
                <form method="POST" action="/jobs">
                    <div class="form-group">
                        <label for="userId">İlanı Veren Mezun (Author):</label>
                        <select name="userId" id="userId" required>
                            {{userOptions}}
                        </select>
                    </div>
                    <div class="form-group">
                        <label for="title">Pozisyon Başlığı (Title):</label>
                        <input type="text" name="title" id="title" placeholder="Örn: Junior Backend Developer" required />
                    </div>
                    <div class="form-group">
                        <label for="company">Şirket (Company):</label>
                        <input type="text" name="company" id="company" placeholder="Örn: Acme Inc." required />
                    </div>
                    <div class="form-group">
                        <label for="location">Konum (Location):</label>
                        <input type="text" name="location" id="location" value="Istanbul / Remote" />
                    </div>
                    <div class="form-group">
                        <label for="description">Açıklama &amp; Gereksinimler:</label>
                        <textarea name="description" id="description" rows="4" placeholder="İlan detayları, aranan nitelikler..." required></textarea>
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
                <tr style="border-bottom: 1px solid #334155;">
                    <td style="padding: 0.6rem;"><strong>#{a.UserId}</strong> {(a.User != null ? a.User.Name : "Applicant")}</td>
                    <td style="padding: 0.6rem;">{(a.User != null ? a.User.Email : "")}</td>
                    <td style="padding: 0.6rem;"><span style="background: rgba(56, 189, 248, 0.15); color: #38bdf8; padding: 0.2rem 0.5rem; border-radius: 4px; font-size: 0.8rem; font-weight: 600;">{a.Status}</span></td>
                    <td style="padding: 0.6rem; color: #94a3b8; font-size: 0.85rem;">{a.CreatedAt:yyyy-MM-dd HH:mm}</td>
                </tr>
            """))
            : "<tr><td colspan=\"4\" style=\"padding: 1rem; color: #94a3b8; text-align: center;\">Henüz başvuru yapılmamış. İlk başvuran siz olun!</td></tr>";

        var userSelect = string.Join("\n", allUsers.Select(u => $"<option value=\"{u.Id}\">{u.Name} (#{u.Id})</option>"));

        return $$"""
        <!DOCTYPE html>
        <html lang="tr">
        <head>
            <meta charset="UTF-8">
            <title>{{job.Title}} — {{job.Company}}</title>
            <style>
                body { font-family: -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, sans-serif; background: #0f172a; color: #f8fafc; padding: 2rem; }
                .container { max-width: 850px; margin: 0 auto; }
                .card { background: #1e293b; border: 1px solid #334155; border-radius: 12px; padding: 2rem; margin-bottom: 2rem; }
                .btn { display: inline-block; padding: 0.5rem 1rem; border-radius: 6px; font-weight: 600; text-decoration: none; cursor: pointer; border: none; }
                .btn-primary { background: #4f46e5; color: #fff; }
                a { color: #38bdf8; text-decoration: none; }
                table { width: 100%; border-collapse: collapse; margin-top: 1rem; }
                th { text-align: left; padding: 0.6rem; color: #94a3b8; border-bottom: 1px solid #334155; background: rgba(0,0,0,0.2); }
            </style>
        </head>
        <body>
            <div class="container">
                <p style="margin-bottom: 1rem;"><a href="/jobs">&larr; Tüm İlanlara Dön</a></p>
                <div class="card">
                    <span style="background: rgba(56, 189, 248, 0.15); color: #38bdf8; padding: 0.2rem 0.6rem; border-radius: 4px; font-weight: 700; font-size: 0.85rem;">{{job.Company}}</span>
                    <h1 style="color: #fff; margin: 0.5rem 0 0.2rem 0;">{{job.Title}}</h1>
                    <p style="color: #94a3b8; font-size: 0.95rem; margin-bottom: 1.5rem;">📍 {{job.Location}} &bull; İlanı Veren: <strong>{{(job.User != null ? job.User.Name : $"User #{job.UserId}")}}</strong> &bull; Tarih: {{job.CreatedAt:yyyy-MM-dd}}</p>
                    
                    <h3 style="color: #fff; margin-bottom: 0.5rem;">İlan Açıklaması:</h3>
                    <p style="color: #cbd5e1; line-height: 1.6; margin-bottom: 2rem;">{{job.Description}}</p>

                    <!-- Quick Apply Box -->
                    <div style="background: #0f172a; border: 1px solid #334155; border-radius: 8px; padding: 1.25rem;">
                        <h4 style="color: #fff; margin-bottom: 0.75rem;">🚀 Bu İlana Başvur:</h4>
                        <form method="POST" action="/jobs/{{job.Id}}/apply" style="display: flex; gap: 0.75rem; align-items: center; flex-wrap: wrap;">
                            <label for="applicantUserId" style="color: #cbd5e1; font-size: 0.9rem;">Başvuran Mezun/Öğrenci:</label>
                            <select name="userId" id="applicantUserId" style="padding: 0.5rem; background: #1e293b; color: #fff; border: 1px solid #334155; border-radius: 6px;">
                                {{userSelect}}
                            </select>
                            <button type="submit" class="btn btn-primary">Başvuruyu Gönder</button>
                        </form>
                    </div>
                </div>

                <div class="card">
                    <h3 style="color: #fff; margin-bottom: 0.5rem;">📋 Gelen Başvurular (Applications - {{applications.Count()}}):</h3>
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
