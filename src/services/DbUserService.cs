using Alumni.Data;
using Alumni.Models;

namespace Alumni.Services;

public class DbUserService : IUserService
{
    private readonly AlumniDbContext _context;

    public DbUserService(AlumniDbContext context)
    {
        _context = context;
    }

    public IEnumerable<User> GetAll()
    {
        return _context.Users.OrderBy(u => u.Id).ToList();
    }

    public User? GetById(int id)
    {
        return _context.Users.Find(id);
    }

    public User Create(CreateUserRequest request)
    {
        var resolvedEmail = !string.IsNullOrWhiteSpace(request.Email)
            ? request.Email.Trim()
            : $"{request.Name.Trim().ToLower().Replace(" ", ".")}@ogr.iu.edu.tr";

        var user = new User
        {
            Name = request.Name.Trim(),
            Email = resolvedEmail,
            GraduationYear = request.GraduationYear > 0 ? request.GraduationYear : DateTime.UtcNow.Year,
            Role = !string.IsNullOrWhiteSpace(request.Role) ? request.Role.Trim() : "alumni",
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        _context.SaveChanges();
        return user;
    }

    public User? Update(int id, UpdateUserRequest request)
    {
        var user = _context.Users.Find(id);
        if (user == null) return null;

        user.Name = request.Name.Trim();
        user.Email = request.Email.Trim();
        user.GraduationYear = request.GraduationYear;
        user.Role = request.Role.Trim();
        user.UpdatedAt = DateTime.UtcNow;

        _context.SaveChanges();
        return user;
    }

    public User? Patch(int id, PatchUserRequest request)
    {
        var user = _context.Users.Find(id);
        if (user == null) return null;

        if (!string.IsNullOrWhiteSpace(request.Name))
            user.Name = request.Name.Trim();

        if (!string.IsNullOrWhiteSpace(request.Email))
            user.Email = request.Email.Trim();

        if (request.GraduationYear.HasValue && request.GraduationYear.Value > 0)
            user.GraduationYear = request.GraduationYear.Value;

        if (!string.IsNullOrWhiteSpace(request.Role))
            user.Role = request.Role.Trim();

        user.UpdatedAt = DateTime.UtcNow;

        _context.SaveChanges();
        return user;
    }

    public bool Delete(int id)
    {
        var user = _context.Users.Find(id);
        if (user == null) return false;

        _context.Users.Remove(user);
        _context.SaveChanges();
        return true;
    }
}
