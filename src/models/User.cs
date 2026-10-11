using System.Collections.Concurrent;

namespace Alumni.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int GraduationYear { get; set; }
    public string Role { get; set; } = "alumni";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // In-memory data store embedded inside Model (as requested: "Create Users Model without database connection it include CRUD functions")
    private static readonly ConcurrentDictionary<int, User> _store = new();
    private static int _nextId = 42;

    static User()
    {
        // Pre-seed initial users
        Create("Elif Kaya", "elif.kaya@ogr.iu.edu.tr", 2024, "alumni");
        Create("Mert Aydın", "mert.aydin@ogr.iu.edu.tr", 2021, "alumni");
        Create("Zeynep Arslan", "zeynep.arslan@ogr.iu.edu.tr", 2019, "alumni");
        Create("Can Öztürk", "can.ozturk@ogr.iu.edu.tr", 2023, "alumni");
    }

    // CRUD Functions inside Model
    public static IEnumerable<User> GetAll()
    {
        return _store.Values.OrderBy(u => u.Id);
    }

    public static User? GetById(int id)
    {
        _store.TryGetValue(id, out var user);
        return user;
    }

    public static User Create(string name, string? email, int graduationYear, string? role)
    {
        int id = Interlocked.Increment(ref _nextId);
        var resolvedEmail = !string.IsNullOrWhiteSpace(email) 
            ? email 
            : $"{name.ToLower().Replace(" ", ".")}@ogr.iu.edu.tr";

        var user = new User
        {
            Id = id,
            Name = name,
            Email = resolvedEmail,
            GraduationYear = graduationYear > 0 ? graduationYear : DateTime.UtcNow.Year,
            Role = !string.IsNullOrWhiteSpace(role) ? role : "alumni",
            CreatedAt = DateTime.UtcNow
        };

        _store[id] = user;
        return user;
    }

    public static User? Update(int id, string name, string email, int graduationYear, string role)
    {
        if (!_store.TryGetValue(id, out var existing))
        {
            return null;
        }

        existing.Name = name;
        existing.Email = email;
        existing.GraduationYear = graduationYear;
        existing.Role = role;
        existing.UpdatedAt = DateTime.UtcNow;

        return existing;
    }

    public static User? Patch(int id, string? name, string? email, int? graduationYear, string? role)
    {
        if (!_store.TryGetValue(id, out var existing))
        {
            return null;
        }

        if (!string.IsNullOrWhiteSpace(name)) existing.Name = name;
        if (!string.IsNullOrWhiteSpace(email)) existing.Email = email;
        if (graduationYear.HasValue && graduationYear.Value > 0) existing.GraduationYear = graduationYear.Value;
        if (!string.IsNullOrWhiteSpace(role)) existing.Role = role;
        existing.UpdatedAt = DateTime.UtcNow;

        return existing;
    }

    public static bool Delete(int id)
    {
        return _store.TryRemove(id, out _);
    }
}

public record CreateUserRequest(
    string Name,
    string? Email,
    int GraduationYear,
    string? Role
);

public record UpdateUserRequest(
    string Name,
    string Email,
    int GraduationYear,
    string Role
);

public record PatchUserRequest(
    string? Name,
    string? Email,
    int? GraduationYear,
    string? Role
);
