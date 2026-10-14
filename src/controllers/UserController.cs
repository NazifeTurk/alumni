using Alumni.Models;
using Microsoft.AspNetCore.Mvc;
using UserModel = Alumni.Models.User;

namespace Alumni.Controllers;

[Route("users")]
public class UserController : Controller
{
    private readonly Alumni.Data.AlumniDbContext _context;

    public UserController(Alumni.Data.AlumniDbContext context)
    {
        _context = context;
    }

    // 5. GET /users -> Listing all users with semantic HTML View
    [HttpGet("")]
    public IActionResult Index()
    {
        IEnumerable<UserModel> users;
        try
        {
            var dbUsers = _context.Users.OrderBy(u => u.Id).ToList();
            users = dbUsers.Any() ? dbUsers : UserModel.GetAll();
        }
        catch
        {
            users = UserModel.GetAll();
        }

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

        var resolvedEmail = !string.IsNullOrWhiteSpace(email)
            ? email
            : $"{name.ToLower().Replace(" ", ".")}@ogr.iu.edu.tr";

        try
        {
            var dbUser = new UserModel
            {
                Name = name,
                Email = resolvedEmail,
                GraduationYear = graduationYear > 0 ? graduationYear : DateTime.UtcNow.Year,
                Role = !string.IsNullOrWhiteSpace(role) ? role : "alumni",
                CreatedAt = DateTime.UtcNow
            };
            _context.Users.Add(dbUser);
            _context.SaveChanges();
        }
        catch
        {
            // Fallback
        }

        UserModel.Create(name, email, graduationYear, role);
        return Redirect("/users");
    }

    // 6. GET /users/{id} -> Single user detail view (R in CRUD)
    [HttpGet("{id:int}")]
    public IActionResult Details(int id)
    {
        UserModel? user = null;
        try
        {
            user = _context.Users.Find(id);
        }
        catch
        {
            // Fallback
        }

        user ??= UserModel.GetById(id);

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
        try
        {
            var dbUser = _context.Users.Find(id);
            if (dbUser != null)
            {
                _context.Users.Remove(dbUser);
                _context.SaveChanges();
            }
        }
        catch
        {
            // Fallback
        }

        UserModel.Delete(id);
        return Redirect("/users");
    }

    // --- HTML View Renderers (Presentation Layer for UserController) ---
    private string RenderUserListView(IEnumerable<UserModel> users)
    {
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
        <html lang="en">
        <head>
            <meta charset="UTF-8">
            <meta name="viewport" content="width=device-width, initial-scale=1.0">
            <title>Alumni Users (View Layer) - Week 04</title>
            <style>
                :root {
                    --bg: #0f172a;
                    --card-bg: #1e293b;
                    --text: #f8fafc;
                    --text-muted: #94a3b8;
                    --primary: #4f46e5;
                    --accent: #10b981;
                    --danger: #ef4444;
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
                .container { max-width: 1000px; margin: 0 auto; }
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
                .card-header {
                    display: flex;
                    justify-content: space-between;
                    align-items: center;
                    margin-bottom: 1rem;
                }
                table { width: 100%; border-collapse: collapse; }
                th, td {
                    text-align: left;
                    padding: 0.85rem 1rem;
                    border-bottom: 1px solid var(--border);
                }
                th { color: var(--text-muted); font-size: 0.85rem; text-transform: uppercase; background: rgba(0,0,0,0.15); }
                .badge-year {
                    background: rgba(79, 70, 229, 0.2);
                    color: #818cf8;
                    padding: 0.2rem 0.5rem;
                    border-radius: 4px;
                    font-weight: 600;
                }
                .badge-role {
                    background: rgba(16, 185, 129, 0.2);
                    color: #34d399;
                    padding: 0.2rem 0.5rem;
                    border-radius: 4px;
                    font-size: 0.85rem;
                }
                .btn {
                    display: inline-block;
                    padding: 0.4rem 0.8rem;
                    border-radius: 6px;
                    font-size: 0.85rem;
                    font-weight: 600;
                    text-decoration: none;
                    cursor: pointer;
                    border: none;
                }
                .btn-primary { background: var(--primary); color: #fff; }
                .btn-secondary { background: #334155; color: #fff; }
                .btn-danger { background: rgba(239, 68, 68, 0.2); color: #f87171; border: 1px solid rgba(239, 68, 68, 0.3); }
                .btn-danger:hover { background: var(--danger); color: #fff; }
                .form-grid {
                    display: grid;
                    grid-template-columns: 2fr 2fr 1fr 1fr auto;
                    gap: 1rem;
                    align-items: end;
                }
                .form-group label {
                    display: block;
                    font-size: 0.85rem;
                    color: var(--text-muted);
                    margin-bottom: 0.3rem;
                }
                .form-control {
                    width: 100%;
                    padding: 0.5rem 0.75rem;
                    border-radius: 6px;
                    border: 1px solid var(--border);
                    background: #0f172a;
                    color: #fff;
                    font-size: 0.9rem;
                }
                .form-control:focus { outline: none; border-color: var(--primary); }
            </style>
        </head>
        <body>
            <div class="container">
                <header>
                    <div>
                        <h1>Alumni Management (View Layer)</h1>
                        <p style="color: var(--text-muted); font-size: 0.9rem;">Week 04 — MVC Architecture: UserController serving HTML Views</p>
                    </div>
                    <nav>
                        <a href="/">Home</a>
                        <a href="/users" class="active">Users (View)</a>
                        <a href="/announcements">Announcements (View)</a>
                        <a href="/api/users">API Users (JSON)</a>
                        <a href="/api/swagger" style="color: #34d399; font-weight: 700;">Swagger ⚡</a>
                    </nav>
                </header>

                <!-- Create User Quick Form -->
                <div class="card">
                    <h3 style="margin-bottom: 1rem; font-size: 1.1rem; color: #fff;">➕ Add New Alumni Record (POST /users)</h3>
                    <form method="POST" action="/users">
                        <div class="form-grid">
                            <div class="form-group">
                                <label>Full Name</label>
                                <input type="text" name="name" class="form-control" placeholder="e.g. Nazife Türk" required>
                            </div>
                            <div class="form-group">
                                <label>Email (Optional)</label>
                                <input type="email" name="email" class="form-control" placeholder="nazife@ogr.iu.edu.tr">
                            </div>
                            <div class="form-group">
                                <label>Grad Year</label>
                                <input type="number" name="graduationYear" class="form-control" value="2026" required>
                            </div>
                            <div class="form-group">
                                <label>Role</label>
                                <input type="text" name="role" class="form-control" value="alumni">
                            </div>
                            <button type="submit" class="btn btn-primary" style="padding: 0.55rem 1.2rem;">Add User</button>
                        </div>
                    </form>
                </div>

                <!-- User Table -->
                <div class="card">
                    <div class="card-header">
                        <h3>📋 Registered Alumni List (GET /users)</h3>
                        <span style="color: var(--text-muted); font-size: 0.9rem;">Total: {{users.Count()}} users</span>
                    </div>
                    <table>
                        <thead>
                            <tr>
                                <th>ID</th>
                                <th>Name</th>
                                <th>Email</th>
                                <th>Year</th>
                                <th>Role</th>
                                <th>Actions</th>
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
        <html lang="en">
        <head>
            <meta charset="UTF-8">
            <title>Create User - Alumni</title>
            <style>
                body { font-family: sans-serif; background: #0f172a; color: #fff; padding: 2rem; display: flex; justify-content: center; }
                .card { background: #1e293b; border-radius: 12px; padding: 2rem; width: 100%; max-width: 500px; border: 1px solid #334155; }
                .form-group { margin-bottom: 1.2rem; }
                label { display: block; margin-bottom: 0.4rem; color: #94a3b8; font-size: 0.9rem; }
                input { width: 100%; padding: 0.6rem; border-radius: 6px; border: 1px solid #334155; background: #0f172a; color: #fff; box-sizing: border-box; }
                .btn { padding: 0.6rem 1.2rem; background: #4f46e5; color: #fff; border: none; border-radius: 6px; cursor: pointer; font-weight: 600; }
                a { color: #94a3b8; text-decoration: none; margin-left: 1rem; }
            </style>
        </head>
        <body>
            <div class="card">
                <h2 style="margin-bottom: 1.5rem;">Create New Alumnus</h2>
                <form method="POST" action="/users">
                    <div class="form-group">
                        <label>Full Name</label>
                        <input type="text" name="name" required>
                    </div>
                    <div class="form-group">
                        <label>Email</label>
                        <input type="email" name="email">
                    </div>
                    <div class="form-group">
                        <label>Graduation Year</label>
                        <input type="number" name="graduationYear" value="2026" required>
                    </div>
                    <div class="form-group">
                        <label>Role</label>
                        <input type="text" name="role" value="alumni">
                    </div>
                    <button type="submit" class="btn">Save Alumnus</button>
                    <a href="/users">Cancel</a>
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
        <html lang="en">
        <head>
            <meta charset="UTF-8">
            <title>User Detail - {{user.Name}}</title>
            <style>
                body { font-family: sans-serif; background: #0f172a; color: #fff; padding: 2rem; display: flex; justify-content: center; }
                .card { background: #1e293b; border-radius: 12px; padding: 2rem; width: 100%; max-width: 550px; border: 1px solid #334155; }
                .row { display: flex; justify-content: space-between; padding: 0.75rem 0; border-bottom: 1px solid #334155; }
                .label { color: #94a3b8; }
                .value { font-weight: 600; }
                .btn { display: inline-block; margin-top: 1.5rem; padding: 0.6rem 1.2rem; background: #4f46e5; color: #fff; border-radius: 6px; text-decoration: none; font-weight: 600; }
            </style>
        </head>
        <body>
            <div class="card">
                <h2>Alumnus Profile #{{user.Id}}</h2>
                <p style="color: #94a3b8; margin-bottom: 1.5rem;">View Layer representation from UserController</p>
                
                <div class="row"><span class="label">Name</span><span class="value">{{user.Name}}</span></div>
                <div class="row"><span class="label">Email</span><span class="value">{{user.Email}}</span></div>
                <div class="row"><span class="label">Graduation Year</span><span class="value">{{user.GraduationYear}}</span></div>
                <div class="row"><span class="label">Role</span><span class="value">{{user.Role}}</span></div>
                <div class="row"><span class="label">Registered At</span><span class="value">{{user.CreatedAt:yyyy-MM-dd HH:mm}} UTC</span></div>

                <a href="/users" class="btn">Back to Users List</a>
            </div>
        </body>
        </html>
        """;
    }
}
