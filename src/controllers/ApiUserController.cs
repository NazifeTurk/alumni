using Alumni.Data;
using Alumni.Models;
using Microsoft.AspNetCore.Mvc;
using UserModel = Alumni.Models.User;

namespace Alumni.Controllers;

[ApiController]
[Route("api/users")]
[Produces("application/json")]
[Tags("Users API (ApiUserController)")]
public class ApiUserController : ControllerBase
{
    private readonly AlumniDbContext _context;

    public ApiUserController(AlumniDbContext context)
    {
        _context = context;
    }

    // GET /api/users -> List all users (JSON)
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UserModel>), StatusCodes.Status200OK)]
    public IActionResult GetAll()
    {
        try
        {
            var users = _context.Users.OrderBy(u => u.Id).ToList();
            if (users.Any()) return Ok(users);
        }
        catch
        {
            // Fallback to in-memory if DB is momentarily unreachable
        }

        return Ok(UserModel.GetAll());
    }

    // GET /api/users/{id} -> Get single user by ID (JSON)
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(UserModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(int id)
    {
        try
        {
            var dbUser = _context.Users.Find(id);
            if (dbUser != null) return Ok(dbUser);
        }
        catch
        {
            // Fallback to in-memory
        }

        var user = UserModel.GetById(id);
        if (user == null)
        {
            return NotFound(new { error = 404, message = $"User with ID '{id}' was not found." });
        }

        return Ok(user);
    }

    // POST /api/users -> Create new user (JSON)
    [HttpPost]
    [ProducesResponseType(typeof(UserModel), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] CreateUserRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest(new { error = 400, message = "Name is required." });
        }

        var resolvedEmail = !string.IsNullOrWhiteSpace(request.Email)
            ? request.Email
            : $"{request.Name.ToLower().Replace(" ", ".")}@ogr.iu.edu.tr";

        try
        {
            var dbUser = new UserModel
            {
                Name = request.Name,
                Email = resolvedEmail,
                GraduationYear = request.GraduationYear > 0 ? request.GraduationYear : DateTime.UtcNow.Year,
                Role = !string.IsNullOrWhiteSpace(request.Role) ? request.Role : "alumni",
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(dbUser);
            _context.SaveChanges();

            // Sync with in-memory model as well
            UserModel.Create(request.Name, request.Email, request.GraduationYear, request.Role);

            return Created($"/api/users/{dbUser.Id}", dbUser);
        }
        catch
        {
            // Fallback to in-memory creation
            var created = UserModel.Create(request.Name, request.Email, request.GraduationYear, request.Role);
            return Created($"/api/users/{created.Id}", created);
        }
    }

    // PUT /api/users/{id} -> Replace entire user (JSON)
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(UserModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(int id, [FromBody] UpdateUserRequest request)
    {
        try
        {
            var dbUser = _context.Users.Find(id);
            if (dbUser != null)
            {
                dbUser.Name = request.Name;
                dbUser.Email = request.Email;
                dbUser.GraduationYear = request.GraduationYear;
                dbUser.Role = request.Role;
                dbUser.UpdatedAt = DateTime.UtcNow;
                _context.SaveChanges();

                UserModel.Update(id, request.Name, request.Email, request.GraduationYear, request.Role);
                return Ok(dbUser);
            }
        }
        catch
        {
            // Fallback
        }

        var updated = UserModel.Update(id, request.Name, request.Email, request.GraduationYear, request.Role);
        if (updated == null)
        {
            return NotFound(new { error = 404, message = $"User with ID '{id}' was not found." });
        }

        return Ok(updated);
    }

    // PATCH /api/users/{id} -> Partial update user (JSON)
    [HttpPatch("{id:int}")]
    [ProducesResponseType(typeof(UserModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Patch(int id, [FromBody] PatchUserRequest request)
    {
        try
        {
            var dbUser = _context.Users.Find(id);
            if (dbUser != null)
            {
                if (!string.IsNullOrWhiteSpace(request.Name)) dbUser.Name = request.Name;
                if (!string.IsNullOrWhiteSpace(request.Email)) dbUser.Email = request.Email;
                if (request.GraduationYear.HasValue && request.GraduationYear.Value > 0) dbUser.GraduationYear = request.GraduationYear.Value;
                if (!string.IsNullOrWhiteSpace(request.Role)) dbUser.Role = request.Role;
                dbUser.UpdatedAt = DateTime.UtcNow;
                _context.SaveChanges();

                UserModel.Patch(id, request.Name, request.Email, request.GraduationYear, request.Role);
                return Ok(dbUser);
            }
        }
        catch
        {
            // Fallback
        }

        var patched = UserModel.Patch(id, request.Name, request.Email, request.GraduationYear, request.Role);
        if (patched == null)
        {
            return NotFound(new { error = 404, message = $"User with ID '{id}' was not found." });
        }

        return Ok(patched);
    }

    // DELETE /api/users/{id} -> Remove user (JSON)
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id)
    {
        try
        {
            var dbUser = _context.Users.Find(id);
            if (dbUser != null)
            {
                _context.Users.Remove(dbUser);
                _context.SaveChanges();
                UserModel.Delete(id);
                return Ok(new { status = "success", message = $"User with ID '{id}' was deleted." });
            }
        }
        catch
        {
            // Fallback
        }

        bool deleted = UserModel.Delete(id);
        if (!deleted)
        {
            return NotFound(new { error = 404, message = $"User with ID '{id}' was not found." });
        }

        return Ok(new { status = "success", message = $"User with ID '{id}' was deleted." });
    }
}
