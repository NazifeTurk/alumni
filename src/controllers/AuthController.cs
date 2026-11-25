using Alumni.Data;
using Alumni.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Alumni.Controllers;

[Route("auth")]
public class AuthController : Controller
{
    private readonly AlumniDbContext _db;

    public AuthController(AlumniDbContext db)
    {
        _db = db;
    }

    // ─── Shared HTML helpers ───────────────────────────────────────────────────

    private static string NavHtml(HttpContext ctx, string activePage = "")
    {
        var name = ctx.Session.GetString("UserName");
        var role = ctx.Session.GetString("UserRole") ?? "";
        bool loggedIn = !string.IsNullOrEmpty(name);

        var adminLink = role == "admin"
            ? """<a href="/admin" class="nav-link" style="color:var(--green);font-weight:700;">🛡️ Admin Panel</a>"""
            : "";

        var profileLink = loggedIn
            ? """<a href="/profile" class="nav-link">Profilim</a>"""
            : "";

        var authLinks = loggedIn
            ? $"""
               <a href="/profile" class="nav-user-badge" style="text-decoration:none;" title="Profilimi Görüntüle">👤 {System.Net.WebUtility.HtmlEncode(name)} <span style="font-size:0.7rem;opacity:0.8;">({role})</span></a>
               <a href="/auth/logout" class="btn btn-ghost" style="font-size:0.82rem;">Çıkış</a>
               """
            : """
              <a href="/auth/login" class="nav-link">Giriş Yap</a>
              <a href="/auth/register" class="btn btn-primary" style="font-size:0.82rem;">Kayıt Ol</a>
              """;

        return $"""
            <header class="nav-wrapper">
              <div class="nav-pill-container">
                <a href="/" class="nav-brand">
                  <div class="nav-brand-logo">
                    <img src="/assets/iu-logo.svg" onerror="this.onerror=null;this.src='https://upload.wikimedia.org/wikipedia/commons/1/1e/%C4%B0stanbul_%C3%9Cniversitesi_Logo.svg';" alt="İÜ">
                  </div>
                  <div class="nav-brand-text">
                    <span class="nav-brand-title">İSTANBUL ÜNİVERSİTESİ</span>
                    <span class="nav-brand-sub">Mezun Takip Sistemi · 1453</span>
                  </div>
                </a>
                <nav class="nav-links">
                  <a href="/users" class="nav-link">Mezunlar</a>
                  <a href="/jobs" class="nav-link">Kariyer</a>
                  <a href="/announcements" class="nav-link">Duyurular</a>
                  {profileLink}
                  {adminLink}
                </nav>
                <div class="nav-actions">{authLinks}</div>
              </div>
            </header>
            """;
    }

    private const string AuthCss = """
        :root {
          --canvas: #FAF9F6; --card: #FFFFFF; --inset: #F5F4EE;
          --hairline: #E2DFD6; --hairline-hover: #C8DBCF;
          --green: #004225; --green-hover: #065A35; --green-tint: #E8F0EB;
          --text: #1C2920; --muted: #55685C; --light: #7E9084;
          --radius-pill: 9999px; --radius-card: 4px;
        }
        *, *::before, *::after { box-sizing: border-box; margin: 0; padding: 0; }
        body {
          font-family: 'Inter', -apple-system, BlinkMacSystemFont, sans-serif;
          background-color: var(--canvas);
          background-image: url('/assets/iu-gate-silhouette.svg');
          background-repeat: no-repeat;
          background-position: center 40px;
          background-size: 1100px auto;
          background-attachment: fixed;
          color: var(--text);
          min-height: 100vh;
        }
        .nav-wrapper { position: sticky; top: 1rem; z-index: 100; display: flex; justify-content: center; padding: 0 1rem; margin-bottom: 2.5rem; }
        .nav-pill-container { display: flex; align-items: center; gap: 1.25rem; background: rgba(250,249,246,0.94); border: 1px solid var(--hairline); border-radius: var(--radius-pill); padding: 0.55rem 1.25rem; backdrop-filter: blur(20px); -webkit-backdrop-filter: blur(20px); box-shadow: none; max-width: 900px; width: 100%; }
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
        .btn { display: inline-flex; align-items: center; gap: 0.4rem; padding: 0.6rem 1.4rem; border-radius: var(--radius-pill); font-size: 0.88rem; font-weight: 600; cursor: pointer; text-decoration: none; border: 1px solid transparent; transition: all 0.15s; }
        .btn-primary { background: var(--green); color: #fff; border-color: var(--green); }
        .btn-primary:hover { background: var(--green-hover); border-color: var(--green-hover); }
        .btn-ghost { background: transparent; color: var(--muted); border-color: var(--hairline); }
        .btn-ghost:hover { background: var(--inset); color: var(--text); }
        .auth-container { max-width: 420px; margin: 0 auto; padding: 1rem; }
        .auth-card { background: var(--card); border: 1px solid var(--hairline); border-radius: var(--radius-card); padding: 2.25rem 2rem; }
        .auth-logo { display: flex; justify-content: center; margin-bottom: 1.5rem; }
        .auth-logo img { width: 52px; height: 52px; object-fit: contain; }
        .auth-title { font-size: 1.35rem; font-weight: 700; color: var(--green); text-align: center; margin-bottom: 0.4rem; }
        .auth-sub { font-size: 0.85rem; color: var(--muted); text-align: center; margin-bottom: 1.75rem; }
        .form-group { margin-bottom: 1rem; }
        .form-group label { display: block; font-size: 0.82rem; font-weight: 600; color: var(--text); margin-bottom: 0.4rem; }
        .form-group input, .form-group select {
          width: 100%; padding: 0.65rem 0.9rem;
          border: 1px solid var(--hairline); border-radius: var(--radius-pill);
          font-family: inherit; font-size: 0.9rem; color: var(--text);
          background: var(--canvas); outline: none;
          transition: border-color 0.15s;
        }
        .form-group input:focus, .form-group select:focus { border-color: var(--green); background: var(--card); }
        .form-submit { width: 100%; padding: 0.75rem; background: var(--green); color: #fff; border: none; border-radius: var(--radius-pill); font-family: inherit; font-size: 0.95rem; font-weight: 700; cursor: pointer; transition: background 0.15s; margin-top: 0.5rem; }
        .form-submit:hover { background: var(--green-hover); }
        .auth-alt { text-align: center; margin-top: 1.25rem; font-size: 0.85rem; color: var(--muted); }
        .auth-alt a { color: var(--green); font-weight: 600; text-decoration: none; }
        .auth-alt a:hover { text-decoration: underline; }
        .alert { padding: 0.75rem 1rem; border-radius: var(--radius-card); margin-bottom: 1rem; font-size: 0.88rem; font-weight: 500; }
        .alert-error { background: #FFE4E6; color: #BE123C; border: 1px solid #FECDD3; }
        .alert-success { background: var(--green-tint); color: var(--green); border: 1px solid var(--hairline-hover); }
        .divider { display: flex; align-items: center; gap: 0.75rem; margin: 1.25rem 0; color: var(--light); font-size: 0.8rem; }
        .divider::before, .divider::after { content: ''; flex: 1; height: 1px; background: var(--hairline); }
        .badge { display: inline-flex; align-items: center; padding: 0.2rem 0.65rem; border-radius: var(--radius-pill); font-size: 0.72rem; font-weight: 700; text-transform: uppercase; letter-spacing: 0.04em; }
        .badge-admin { background: var(--green-tint); color: var(--green); border: 1px solid var(--hairline-hover); }
        .badge-alumni { background: #FEF3C7; color: #B45309; border: 1px solid #FDE68A; }
        .badge-student { background: #E0F2FE; color: #0284C7; border: 1px solid #BAE6FD; }
        .badge-faculty { background: #F3E8FF; color: #7C3AED; border: 1px solid #DDD6FE; }
    """;

    private static string PageShell(HttpContext ctx, string title, string body, string activePage = "")
    {
        return $$"""
            <!DOCTYPE html>
            <html lang="tr">
            <head>
              <meta charset="UTF-8">
              <meta name="viewport" content="width=device-width, initial-scale=1.0">
              <title>{{title}} — İstanbul Üniversitesi</title>
              <link rel="preconnect" href="https://fonts.googleapis.com">
              <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
              <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700;800&display=swap" rel="stylesheet">
              <style>
                {{AuthCss}}
              </style>
            </head>
            <body>
              {{NavHtml(ctx, activePage)}}
              {{body}}
            </body>
            </html>
            """;
    }

    // ─── GET /auth/register ────────────────────────────────────────────────────
    [HttpGet("register")]
    public IActionResult Register([FromQuery] string? error)
    {
        if (!string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
            return Redirect("/");

        var errorHtml = error != null
            ? $"""<div class="alert alert-error">⚠️ {System.Net.WebUtility.HtmlEncode(error)}</div>"""
            : "";

        var body = $"""
            <div class="auth-container">
              <div class="auth-card">
                <div class="auth-logo">
                  <img src="/assets/iu-logo.svg" onerror="this.onerror=null;this.src='https://upload.wikimedia.org/wikipedia/commons/1/1e/%C4%B0stanbul_%C3%9Cniversitesi_Logo.svg';" alt="İÜ">
                </div>
                <h1 class="auth-title">Hesap Oluştur</h1>
                <p class="auth-sub">İstanbul Üniversitesi Mezun Ağına katılın</p>
                {errorHtml}
                <form method="POST" action="/auth/register">
                  <div class="form-group">
                    <label>Ad Soyad</label>
                    <input type="text" name="name" placeholder="Nazife Türk" required>
                  </div>
                  <div class="form-group">
                    <label>E-posta</label>
                    <input type="email" name="email" placeholder="nazife@ogr.iu.edu.tr" required>
                  </div>
                  <div class="form-group">
                    <label>Bölüm / Fakülte</label>
                    <select name="department">
                      <option value="">Seçiniz...</option>
                      <option>Bilgisayar Mühendisliği</option>
                      <option>İşletme</option>
                      <option>Hukuk</option>
                      <option>Tıp</option>
                      <option>Psikoloji</option>
                      <option>İktisat</option>
                      <option>Edebiyat</option>
                      <option>Fen Fakültesi</option>
                      <option>Diğer</option>
                    </select>
                  </div>
                  <div class="form-group">
                    <label>Mezuniyet Yılı</label>
                    <input type="number" name="graduationYear" placeholder="2024" min="1950" max="2030" required>
                  </div>
                  <div class="form-group">
                    <label>Şifre</label>
                    <input type="password" name="password" placeholder="En az 6 karakter" minlength="6" required>
                  </div>
                  <div class="form-group">
                    <label>Şifre Tekrar</label>
                    <input type="password" name="passwordConfirm" placeholder="Şifreyi tekrar girin" required>
                  </div>
                  <button type="submit" class="form-submit">Kayıt Ol</button>
                </form>
                <div class="auth-alt">Zaten hesabınız var mı? <a href="/auth/login">Giriş Yap</a></div>
              </div>
            </div>
            """;

        return Content(PageShell(HttpContext, "Kayıt Ol", body, "register"), "text/html");
    }

    // ─── POST /auth/register ───────────────────────────────────────────────────
    [HttpPost("register")]
    public async Task<IActionResult> RegisterPost(
        [FromForm] string name,
        [FromForm] string email,
        [FromForm] string? department,
        [FromForm] int graduationYear,
        [FromForm] string password,
        [FromForm] string passwordConfirm)
    {
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            return Redirect("/auth/register?error=Lütfen+tüm+alanları+doldurun.");

        if (password != passwordConfirm)
            return Redirect("/auth/register?error=Şifreler+eşleşmiyor.");

        if (password.Length < 6)
            return Redirect("/auth/register?error=Şifre+en+az+6+karakter+olmalıdır.");

        var exists = await _db.Users.AnyAsync(u => u.Email == email);
        if (exists)
            return Redirect("/auth/register?error=Bu+e-posta+zaten+kayıtlı.");

        var user = new User
        {
            Name = name,
            Email = email,
            Department = department ?? "",
            GraduationYear = graduationYear > 0 ? graduationYear : DateTime.UtcNow.Year,
            Role = "alumni",
            PasswordHash = HashPassword(password),
            CreatedAt = DateTime.UtcNow
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        // Auto-login after register
        HttpContext.Session.SetString("UserId", user.Id.ToString());
        HttpContext.Session.SetString("UserName", user.Name);
        HttpContext.Session.SetString("UserEmail", user.Email);
        HttpContext.Session.SetString("UserRole", user.Role);

        return Redirect("/");
    }

    // ─── GET /auth/login ───────────────────────────────────────────────────────
    [HttpGet("login")]
    public IActionResult Login([FromQuery] string? error, [FromQuery] string? returnUrl)
    {
        if (!string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
            return Redirect(returnUrl ?? "/");

        var errorHtml = error != null
            ? $"""<div class="alert alert-error">⚠️ {System.Net.WebUtility.HtmlEncode(error)}</div>"""
            : "";

        var body = $"""
            <div class="auth-container">
              <div class="auth-card">
                <div class="auth-logo">
                  <img src="/assets/iu-logo.svg" onerror="this.onerror=null;this.src='https://upload.wikimedia.org/wikipedia/commons/1/1e/%C4%B0stanbul_%C3%9Cniversitesi_Logo.svg';" alt="İÜ">
                </div>
                <h1 class="auth-title">Giriş Yap</h1>
                <p class="auth-sub">Mezun Takip Sistemine Hoş Geldiniz</p>
                {errorHtml}
                <form method="POST" action="/auth/login{(returnUrl != null ? "?returnUrl=" + Uri.EscapeDataString(returnUrl) : "")}">
                  <div class="form-group">
                    <label>E-posta</label>
                    <input type="email" name="email" placeholder="nazife@ogr.iu.edu.tr" required autofocus>
                  </div>
                  <div class="form-group">
                    <label>Şifre</label>
                    <input type="password" name="password" placeholder="••••••••" required>
                  </div>
                  <button type="submit" class="form-submit">Giriş Yap</button>
                </form>
                <div class="divider">veya</div>
                <div class="auth-alt">Hesabınız yok mu? <a href="/auth/register">Kayıt Ol</a></div>
                <p style="margin-top:1rem;font-size:0.78rem;color:var(--light);text-align:center;">
                  🛡️ Admin: admin@iu.edu.tr / admin1453
                </p>
              </div>
            </div>
            """;

        return Content(PageShell(HttpContext, "Giriş Yap", body, "login"), "text/html");
    }

    // ─── POST /auth/login ──────────────────────────────────────────────────────
    [HttpPost("login")]
    public async Task<IActionResult> LoginPost(
        [FromForm] string email,
        [FromForm] string password,
        [FromQuery] string? returnUrl)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            return Redirect("/auth/login?error=E-posta+ve+şifre+gereklidir.");

        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null || !VerifyPassword(password, user.PasswordHash))
            return Redirect("/auth/login?error=E-posta+veya+şifre+hatalı.");

        HttpContext.Session.SetString("UserId", user.Id.ToString());
        HttpContext.Session.SetString("UserName", user.Name);
        HttpContext.Session.SetString("UserEmail", user.Email);
        HttpContext.Session.SetString("UserRole", user.Role);

        return Redirect(returnUrl ?? "/");
    }

    // ─── GET /auth/logout ──────────────────────────────────────────────────────
    [HttpGet("logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return Redirect("/");
    }

    // ─── GET /profile (User-Specific Dashboard & Settings) ───────────────────
    [HttpGet("/profile")]
    public async Task<IActionResult> Profile([FromQuery] string? msg, [FromQuery] string? error)
    {
        var userIdStr = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out var userId))
            return Redirect("/auth/login?returnUrl=/profile");

        var user = await _db.Users
            .Include(u => u.Jobs)
            .Include(u => u.Applications)
                .ThenInclude(a => a.JobPosting)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            return Redirect("/auth/logout");

        var msgHtml = msg != null
            ? $"""<div class="alert alert-success">✓ {System.Net.WebUtility.HtmlEncode(msg)}</div>"""
            : "";
        var errHtml = error != null
            ? $"""<div class="alert alert-error">⚠️ {System.Net.WebUtility.HtmlEncode(error)}</div>"""
            : "";

        var roleBadgeClass = user.Role switch
        {
            "admin" => "badge-admin",
            "student" => "badge-student",
            "faculty" => "badge-faculty",
            _ => "badge-alumni"
        };

        var userJobsHtml = user.Jobs.Any()
            ? string.Join("", user.Jobs.Select(j => $"""
                <div style="padding:0.75rem;border-bottom:1px solid #EBE8E0;display:flex;justify-content:space-between;align-items:center;">
                  <div>
                    <strong style="color:var(--text);font-size:0.9rem;">{System.Net.WebUtility.HtmlEncode(j.Title)}</strong>
                    <div style="font-size:0.78rem;color:var(--muted);">{System.Net.WebUtility.HtmlEncode(j.Company)} · {System.Net.WebUtility.HtmlEncode(j.City)}</div>
                  </div>
                  <a href="/jobs/{j.Id}" class="btn btn-ghost" style="padding:0.3rem 0.75rem;font-size:0.75rem;">Görüntüle</a>
                </div>
              """))
            : "<p style='color:var(--muted);font-size:0.85rem;padding:0.5rem 0;'>Henüz yayınladığınız bir ilan bulunmuyor.</p>";

        var body = $"""
            <div style="max-width: 860px; margin: 0 auto; padding: 0 1rem 3rem;">
              <div style="display:flex;align-items:center;justify-content:space-between;margin-bottom:1.5rem;flex-wrap:wrap;gap:1rem;">
                <div>
                  <h1 style="font-size:1.75rem;font-weight:800;color:var(--green);">Profilim &amp; Kullanıcı Paneli</h1>
                  <p style="font-size:0.88rem;color:var(--muted);margin-top:0.25rem;">Kişisel bilgilerinizi ve hesap ayarlarınızı yönetin</p>
                </div>
                <div style="display:flex;gap:0.5rem;align-items:center;">
                  <span class="badge {roleBadgeClass}" style="padding:0.35rem 0.85rem;font-size:0.8rem;">{user.Role.ToUpper()}</span>
                  <a href="/auth/logout" class="btn btn-ghost" style="font-size:0.8rem;">Çıkış Yap</a>
                </div>
              </div>

              {msgHtml}
              {errHtml}

              <div style="display:grid;grid-template-columns:repeat(auto-fit, minmax(320px, 1fr));gap:1.5rem;">
                <!-- Profile Edit Card -->
                <div class="auth-card" style="padding:1.75rem;">
                  <h2 style="font-size:1.15rem;font-weight:700;color:var(--green);margin-bottom:1.25rem;display:flex;align-items:center;gap:0.5rem;">
                    <span>👤</span> Kişisel Bilgiler
                  </h2>
                  <form method="POST" action="/profile/update">
                    <div class="form-group">
                      <label>Ad Soyad</label>
                      <input type="text" name="name" value="{System.Net.WebUtility.HtmlEncode(user.Name)}" required>
                    </div>
                    <div class="form-group">
                      <label>E-posta Adresi (Değiştirilemez)</label>
                      <input type="email" value="{System.Net.WebUtility.HtmlEncode(user.Email)}" disabled style="opacity:0.7;cursor:not-allowed;">
                    </div>
                    <div class="form-group">
                      <label>Bölüm / Fakülte</label>
                      <input type="text" name="department" value="{System.Net.WebUtility.HtmlEncode(user.Department)}" placeholder="Örn: Bilgisayar Mühendisliği">
                    </div>
                    <div class="form-group">
                      <label>Mezuniyet Yılı</label>
                      <input type="number" name="graduationYear" value="{user.GraduationYear}" min="1950" max="2030" required>
                    </div>
                    <div class="form-group">
                      <label>Yeni Şifre (Boş bırakırsanız değişmez)</label>
                      <input type="password" name="newPassword" placeholder="••••••••" minlength="6">
                    </div>
                    <button type="submit" class="form-submit" style="margin-top:1rem;">Bilgileri Güncelle</button>
                  </form>
                </div>

                <!-- Account Summary Card -->
                <div style="display:flex;flex-direction:column;gap:1.5rem;">
                  <div class="auth-card" style="padding:1.75rem;">
                    <h2 style="font-size:1.15rem;font-weight:700;color:var(--green);margin-bottom:1rem;display:flex;align-items:center;gap:0.5rem;">
                      <span>🏛️</span> Hesap Künyesi
                    </h2>
                    <div style="display:grid;grid-template-columns:1fr 1fr;gap:0.75rem;margin-bottom:1rem;">
                      <div style="background:var(--inset);padding:0.75rem;border-radius:4px;border:1px solid var(--hairline);">
                        <div style="font-size:0.72rem;color:var(--muted);text-transform:uppercase;font-weight:700;">Kullanıcı No</div>
                        <div style="font-size:1.1rem;font-weight:800;color:var(--green);margin-top:0.2rem;">#{user.Id}</div>
                      </div>
                      <div style="background:var(--inset);padding:0.75rem;border-radius:4px;border:1px solid var(--hairline);">
                        <div style="font-size:0.72rem;color:var(--muted);text-transform:uppercase;font-weight:700;">Sistem Rolü</div>
                        <div style="font-size:1.1rem;font-weight:800;color:var(--green);margin-top:0.2rem;">{user.Role}</div>
                      </div>
                    </div>
                    <div style="font-size:0.82rem;color:var(--muted);line-height:1.6;">
                      <div><strong>Kayıt Tarihi:</strong> {user.CreatedAt:dd MMMM yyyy HH:mm}</div>
                      <div><strong>Son Güncelleme:</strong> {(user.UpdatedAt.HasValue ? user.UpdatedAt.Value.ToString("dd MMMM yyyy HH:mm") : "İlk kayıt")}</div>
                    </div>
                  </div>

                  <!-- My Postings Card -->
                  <div class="auth-card" style="padding:1.75rem;">
                    <div style="display:flex;justify-content:space-between;align-items:center;margin-bottom:0.75rem;">
                      <h2 style="font-size:1.15rem;font-weight:700;color:var(--green);display:flex;align-items:center;gap:0.5rem;">
                        <span>💼</span> İlanlarım ({user.Jobs.Count})
                      </h2>
                      <a href="/jobs/create" class="btn btn-primary" style="padding:0.35rem 0.85rem;font-size:0.75rem;">+ İlan Ver</a>
                    </div>
                    <div>{userJobsHtml}</div>
                  </div>
                </div>
              </div>
            </div>
            """;

        return Content(PageShell(HttpContext, "Profilim", body, "profile"), "text/html");
    }

    // ─── POST /profile/update ──────────────────────────────────────────────────
    [HttpPost("/profile/update")]
    public async Task<IActionResult> ProfileUpdate(
        [FromForm] string name,
        [FromForm] string? department,
        [FromForm] int graduationYear,
        [FromForm] string? newPassword)
    {
        var userIdStr = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out var userId))
            return Redirect("/auth/login?returnUrl=/profile");

        if (string.IsNullOrWhiteSpace(name))
            return Redirect("/profile?error=İsim+alanı+boş+bırakılamaz.");

        var user = await _db.Users.FindAsync(userId);
        if (user == null) return Redirect("/auth/logout");

        user.Name = name.Trim();
        user.Department = department?.Trim() ?? "";
        user.GraduationYear = graduationYear > 0 ? graduationYear : user.GraduationYear;
        user.UpdatedAt = DateTime.UtcNow;

        if (!string.IsNullOrWhiteSpace(newPassword))
        {
            if (newPassword.Length < 6)
                return Redirect("/profile?error=Yeni+şifre+en+az+6+karakter+olmalıdır.");
            user.PasswordHash = HashPassword(newPassword);
        }

        await _db.SaveChangesAsync();

        // Update active session name
        HttpContext.Session.SetString("UserName", user.Name);

        return Redirect("/profile?msg=Profil+bilgileriniz+başarıyla+güncellendi.");
    }

    // ─── Password helpers ──────────────────────────────────────────────────────
    public static string HashPassword(string password)
    {
        var salt = "IU_ALUMNI_SALT_1453";
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(salt + password));
        return Convert.ToHexString(bytes).ToLower();
    }

    public static bool VerifyPassword(string password, string hash)
    {
        if (hash.StartsWith("$2a$")) return false;
        return HashPassword(password) == hash;
    }
}
