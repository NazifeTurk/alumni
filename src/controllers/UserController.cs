using Alumni.Models;
using Alumni.Services;
using Microsoft.AspNetCore.Mvc;
using UserModel = Alumni.Models.User;

namespace Alumni.Controllers;

[Route("users")]
public class UserController : Controller
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    // 5. GET /users -> Listing all users with semantic HTML View
    [HttpGet("")]
    public IActionResult Index()
    {
        var users = _userService.GetAll();
        return Content(RenderUserListView(users), "text/html");
    }

    // 5. GET /users/create -> Render Create Form View
    [HttpGet("create")]
    public IActionResult CreateForm()
    {
        return Content(RenderCreateUserForm(), "text/html");
    }

    // 5. POST /users -> Create user from HTML Form and redirect to list (C in CRUD)
    [HttpPost("")]
    public IActionResult Create([FromForm] string name, [FromForm] string? email, [FromForm] int graduationYear, [FromForm] string? role)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return BadRequest("Name is required.");
        }

        _userService.Create(new CreateUserRequest(name, email, graduationYear, role));
        return Redirect("/users");
    }

    // 6. GET /users/{id} -> Single user detail view (R in CRUD)
    [HttpGet("{id:int}")]
    public IActionResult Details(int id)
    {
        var user = _userService.GetById(id);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        return Content(RenderUserDetailView(user), "text/html");
    }

    // 6. POST /users/{id}/delete -> Delete user from HTML view action (D in CRUD)
    [HttpPost("{id:int}/delete")]
    public IActionResult Delete(int id)
    {
        _userService.Delete(id);
        return Redirect("/users");
    }

    // --- HTML View Renderers (Presentation Layer for UserController) ---
    private string RenderUserListView(IEnumerable<UserModel> users)
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

        var rows = string.Join("\n", users.Select(u => $"""
            <tr>
                <td><strong>#{u.Id}</strong></td>
                <td>{u.Name}</td>
                <td>{u.Email}</td>
                <td><span class="badge-year">{u.GraduationYear}</span></td>
                <td><span class="badge-role">{u.Role}</span></td>
                <td>
                    <div style="display:flex;gap:0.5rem;">
                        <a href="/users/{u.Id}" class="btn btn-secondary">View</a>
                        <form method="POST" action="/users/{u.Id}/delete" onsubmit="return confirm('Delete {u.Name}?');" style="margin:0;">
                            <button type="submit" class="btn btn-danger">Delete</button>
                        </form>
                    </div>
                </td>
            </tr>
        """));

        return $$"""
        <!DOCTYPE html>
        <html lang="tr">
        <head>
            <meta charset="UTF-8">
            <meta name="viewport" content="width=device-width, initial-scale=1.0">
            <title>Mezun Rehberi (Users) — İstanbul Üniversitesi</title>
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
                    --danger: #BE123C;
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
                .container { max-width: 1040px; margin: 0 auto; }
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
                h1 { font-family: 'Inter', sans-serif; font-size: 2rem; font-weight: 400; color: var(--primary); letter-spacing: -0.02em; }
                nav { display: flex; align-items: center; gap: 1.25rem; flex-wrap: wrap; }
                nav a {
                    color: var(--text-muted);
                    text-decoration: none;
                    font-size: 0.9rem;
                    font-weight: 600;
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
                .card-header {
                    display: flex;
                    justify-content: space-between;
                    align-items: center;
                    margin-bottom: 1.25rem;
                }
                table { width: 100%; border-collapse: collapse; }
                th, td {
                    text-align: left;
                    padding: 0.85rem 1rem;
                    border-bottom: 1px solid var(--border);
                    font-size: 0.9rem;
                }
                th { color: var(--text-muted); font-size: 0.78rem; text-transform: uppercase; background: #F5F4EE; letter-spacing: 0.04em; }
                .badge-year {
                    background: var(--pill-bg);
                    color: var(--primary);
                    padding: 0.2rem 0.55rem;
                    border-radius: 9999px;
                    font-weight: 700;
                    font-size: 0.8rem;
                    border: 1px solid var(--pill-border);
                }
                .badge-role {
                    background: #FEF3C7;
                    color: #92400E;
                    padding: 0.2rem 0.55rem;
                    border-radius: 9999px;
                    font-size: 0.8rem;
                    font-weight: 600;
                }
                .btn {
                    display: inline-flex;
                    align-items: center;
                    justify-content: center;
                    padding: 0.45rem 1rem;
                    border-radius: 9999px;
                    font-size: 0.85rem;
                    font-weight: 700;
                    text-decoration: none;
                    cursor: pointer;
                    border: 1px solid transparent;
                    transition: background 0.15s;
                }
                .btn-primary { background: var(--primary); color: #fff; }
                .btn-primary:hover { background: var(--primary-hover); }
                .btn-secondary { background: #FFFFFF; color: var(--primary); border: 1px solid var(--border); }
                .btn-secondary:hover { background: var(--pill-bg); }
                .btn-danger { background: #FFE4E6; color: var(--danger); border: 1px solid #FECDD3; }
                .btn-danger:hover { background: #F43F5E; color: #fff; }
                .form-grid {
                    display: grid;
                    grid-template-columns: 2fr 2fr 1fr 1fr auto;
                    gap: 1rem;
                    align-items: end;
                }
                .form-group label {
                    display: block;
                    font-size: 0.82rem;
                    font-weight: 600;
                    color: var(--text-muted);
                    margin-bottom: 0.35rem;
                    text-transform: uppercase;
                    letter-spacing: 0.03em;
                }
                .form-control {
                    width: 100%;
                    padding: 0.55rem 0.75rem;
                    border-radius: 4px;
                    border: 1px solid var(--border);
                    background: #FFFFFF;
                    color: var(--text);
                    font-size: 0.9rem;
                }
                .form-control:focus { outline: none; border-color: var(--primary); }
            </style>
        </head>
        <body>
            <div class="container">
                <header>
                    <div class="header-brand">
                        <img src="/assets/iu-logo.svg" onerror="this.onerror=null; this.src='https://upload.wikimedia.org/wikipedia/commons/1/1e/%C4%B0stanbul_%C3%9Cniversitesi_Logo.svg';" alt="IU Logo">
                        <div>
                            <h1>Mezun Yönetimi (Rehber)</h1>
                            <p style="color: var(--text-muted); font-size: 0.85rem;">İstanbul Üniversitesi &bull; Nazife Türk &bull; MVC UserController</p>
                        </div>
                    </div>
                    <nav>
                        <a href="/">Ana Sayfa</a>
                        <a href="/users" class="active">Mezunlar</a>
                        <a href="/jobs">İş İlanları</a>
                        <a href="/announcements">Duyurular</a>
                        {profileLink}
                        {adminLink}
                        <a href="/swagger" style="color: var(--primary); font-weight: 700;">Swagger API</a>
                        {authActions}
                    </nav>
                </header>

                <!-- Create User Quick Form -->
                <div class="card">
                    <h3 style="margin-bottom: 1rem; font-size: 1.1rem; color: var(--primary);">➕ Yeni Mezun Kaydı Ekle (POST /users)</h3>
                    <form method="POST" action="/users">
                        <div class="form-grid">
                            <div class="form-group">
                                <label>Ad Soyad</label>
                                <input type="text" name="name" class="form-control" placeholder="Örn: Nazife Türk" required>
                            </div>
                            <div class="form-group">
                                <label>E-posta</label>
                                <input type="email" name="email" class="form-control" placeholder="nazife@ogr.iu.edu.tr">
                            </div>
                            <div class="form-group">
                                <label>Mezuniyet Yılı</label>
                                <input type="number" name="graduationYear" class="form-control" value="2026" required>
                            </div>
                            <div class="form-group">
                                <label>Rol / Unvan</label>
                                <input type="text" name="role" class="form-control" value="alumni">
                            </div>
                            <button type="submit" class="btn btn-primary" style="padding: 0.55rem 1.4rem;">Kaydet</button>
                        </div>
                    </form>
                </div>

                <!-- User Table -->
                <div class="card">
                    <div class="card-header">
                        <h3 style="color: var(--primary);">📋 Kayıtlı Mezun Listesi (GET /users)</h3>
                        <span style="color: var(--text-muted); font-size: 0.85rem; font-weight: 600;">Toplam: {{users.Count()}} mezun</span>
                    </div>
                    <table>
                        <thead>
                            <tr>
                                <th>ID</th>
                                <th>Ad Soyad</th>
                                <th>E-posta</th>
                                <th>Mezuniyet</th>
                                <th>Rol</th>
                                <th>İşlemler</th>
                            </tr>
                        </thead>
                        <tbody>
                            {{rows}}
                        </tbody>
                    </table>
                </div>
            </div>
        </body>
        </html>
        """;
    }

    private string RenderCreateUserForm()
    {
        return """
        <!DOCTYPE html>
        <html lang="tr">
        <head>
            <meta charset="UTF-8">
            <title>Yeni Mezun Ekle — İstanbul Üniversitesi</title>
            <style>
                body { font-family: sans-serif; background: #FAF9F6; color: #1C2920; padding: 2rem; display: flex; justify-content: center; }
                .card { background: #FFFFFF; border-radius: 4px; padding: 2rem; width: 100%; max-width: 500px; border: 1px solid #E2DFD6; box-shadow: none; }
                .logo { display: flex; align-items: center; gap: 0.75rem; margin-bottom: 1.5rem; }
                .logo img { width: 40px; height: 40px; }
                .form-group { margin-bottom: 1.2rem; }
                label { display: block; margin-bottom: 0.4rem; color: #55685C; font-size: 0.85rem; font-weight: 600; text-transform: uppercase; }
                input { width: 100%; padding: 0.6rem; border-radius: 4px; border: 1px solid #E2DFD6; background: #FFFFFF; color: #1C2920; box-sizing: border-box; }
                input:focus { outline: none; border-color: #004225; }
                .btn { padding: 0.65rem 1.4rem; background: #004225; color: #fff; border: none; border-radius: 9999px; cursor: pointer; font-weight: 700; text-decoration: none; display: inline-block; }
                .btn:hover { background: #065A35; }
                a.cancel { color: #55685C; text-decoration: none; margin-left: 1rem; font-size: 0.9rem; }
            </style>
        </head>
        <body>
            <div class="card">
                <div class="logo">
                    <img src="/assets/iu-logo.svg" onerror="this.src='https://upload.wikimedia.org/wikipedia/commons/1/1e/%C4%B0stanbul_%C3%9Cniversitesi_Logo.svg';" alt="IU">
                    <h2 style="color: #004225; font-size: 1.35rem;">Yeni Mezun Kaydı</h2>
                </div>
                <form method="POST" action="/users">
                    <div class="form-group">
                        <label>Ad Soyad</label>
                        <input type="text" name="name" required placeholder="Örn: Nazife Türk">
                    </div>
                    <div class="form-group">
                        <label>E-posta</label>
                        <input type="email" name="email" placeholder="nazife@ogr.iu.edu.tr">
                    </div>
                    <div class="form-group">
                        <label>Mezuniyet Yılı</label>
                        <input type="number" name="graduationYear" value="2026" required>
                    </div>
                    <div class="form-group">
                        <label>Rol / Unvan</label>
                        <input type="text" name="role" value="alumni">
                    </div>
                    <button type="submit" class="btn">Mezunu Kaydet</button>
                    <a href="/users" class="cancel">İptal</a>
                </form>
            </div>
        </body>
        </html>
        """;
    }

    private string RenderUserDetailView(UserModel user)
    {
        return $$"""
        <!DOCTYPE html>
        <html lang="tr">
        <head>
            <meta charset="UTF-8">
            <title>Mezun Profili — {{user.Name}}</title>
            <style>
                body { font-family: sans-serif; background: #FAF9F6; color: #1C2920; padding: 2rem; display: flex; justify-content: center; }
                .card { background: #FFFFFF; border-radius: 4px; padding: 2.25rem; width: 100%; max-width: 550px; border: 1px solid #E2DFD6; box-shadow: none; }
                .logo { display: flex; align-items: center; gap: 0.75rem; margin-bottom: 1.5rem; }
                .logo img { width: 44px; height: 44px; }
                .row { display: flex; justify-content: space-between; padding: 0.75rem 0; border-bottom: 1px solid #EBE8E0; font-size: 0.95rem; }
                .label { color: #55685C; font-weight: 600; }
                .value { font-weight: 700; color: #004225; }
                .btn { display: inline-block; margin-top: 1.5rem; padding: 0.65rem 1.4rem; background: #004225; color: #fff; border-radius: 9999px; text-decoration: none; font-weight: 700; }
                .btn:hover { background: #065A35; }
            </style>
        </head>
        <body>
            <div class="card">
                <div class="logo">
                    <img src="/assets/iu-logo.svg" onerror="this.src='https://upload.wikimedia.org/wikipedia/commons/1/1e/%C4%B0stanbul_%C3%9Cniversitesi_Logo.svg';" alt="IU">
                    <div>
                        <h2 style="color: #004225; font-size: 1.4rem;">Mezun Profili #{{user.Id}}</h2>
                        <span style="color: #55685C; font-size: 0.85rem;">İstanbul Üniversitesi &bull; Nazife Türk</span>
                    </div>
                </div>
                
                <div class="row"><span class="label">Ad Soyad</span><span class="value">{{user.Name}}</span></div>
                <div class="row"><span class="label">E-posta</span><span class="value">{{user.Email}}</span></div>
                <div class="row"><span class="label">Mezuniyet Yılı</span><span class="value">{{user.GraduationYear}}</span></div>
                <div class="row"><span class="label">Rol / Unvan</span><span class="value">{{user.Role}}</span></div>
                <div class="row"><span class="label">Kayıt Tarihi</span><span class="value">{{user.CreatedAt:yyyy-MM-dd HH:mm}} UTC</span></div>

                <a href="/users" class="btn">&larr; Mezunlar Listesine Dön</a>
            </div>
        </body>
        </html>
        """;
    }
}
