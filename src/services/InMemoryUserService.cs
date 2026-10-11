using System.Collections.Concurrent;
using Alumni.Models;

namespace Alumni.Services;

public class InMemoryUserService
{
    private readonly ConcurrentDictionary<int, User> _users = new();
    private int _nextId = 42; // Start around 42 as in the professor's slide

    public InMemoryUserService()
    {
        // Pre-seed with the exact 4 alumni from the Week 03 lecture slide
        Seed(new User { Name = "Elif Kaya", Email = "elif.kaya@ogr.iu.edu.tr", GraduationYear = 2024, Role = "alumni" });
        Seed(new User { Name = "Mert Aydın", Email = "mert.aydin@ogr.iu.edu.tr", GraduationYear = 2021, Role = "alumni" });
        Seed(new User { Name = "Zeynep Arslan", Email = "zeynep.arslan@ogr.iu.edu.tr", GraduationYear = 2019, Role = "alumni" });
        Seed(new User { Name = "Can Öztürk", Email = "can.ozturk@ogr.iu.edu.tr", GraduationYear = 2023, Role = "alumni" });
    }

    private void Seed(User user)
    {
        user.Id = _nextId++;
        user.CreatedAt = DateTime.UtcNow.AddMonths(-new Random().Next(1, 12));
        _users[user.Id] = user;
    }

    public IEnumerable<User> GetAll()
    {
        return _users.Values.OrderBy(u => u.Id);
    }

    public User? GetById(int id)
    {
        _users.TryGetValue(id, out var user);
        return user;
    }

    public User Create(CreateUserRequest request)
    {
        var id = Interlocked.Increment(ref _nextId);
        var email = !string.IsNullOrWhiteSpace(request.Email) 
            ? request.Email 
            : $"{request.Name.ToLower().Replace(" ", ".")}@ogr.iu.edu.tr";

        var user = new User
        {
            Id = id,
            Name = request.Name,
            Email = email,
            GraduationYear = request.GraduationYear > 0 ? request.GraduationYear : DateTime.UtcNow.Year,
            Role = !string.IsNullOrWhiteSpace(request.Role) ? request.Role : "alumni",
            CreatedAt = DateTime.UtcNow
        };

        _users[id] = user;
        return user;
    }

    public User? Update(int id, UpdateUserRequest request)
    {
        if (!_users.TryGetValue(id, out var existing))
        {
            return null;
        }

        // Full replacement (PUT semantics)
        existing.Name = request.Name;
        existing.Email = request.Email;
        existing.GraduationYear = request.GraduationYear;
        existing.Role = request.Role;
        existing.UpdatedAt = DateTime.UtcNow;

        return existing;
    }

    public User? Patch(int id, PatchUserRequest request)
    {
        if (!_users.TryGetValue(id, out var existing))
        {
            return null;
        }

        // Partial modification (PATCH semantics)
        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            existing.Name = request.Name;
        }

        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            existing.Email = request.Email;
        }

        if (request.GraduationYear.HasValue && request.GraduationYear.Value > 0)
        {
            existing.GraduationYear = request.GraduationYear.Value;
        }

        if (!string.IsNullOrWhiteSpace(request.Role))
        {
            existing.Role = request.Role;
        }

        existing.UpdatedAt = DateTime.UtcNow;
        return existing;
    }

    public bool Delete(int id)
    {
        return _users.TryRemove(id, out _);
    }
}
