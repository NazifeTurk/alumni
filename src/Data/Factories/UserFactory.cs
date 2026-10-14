using Alumni.Models;

namespace Alumni.Data.Factories;

public static class UserFactory
{
    public static User Create(string name, string? email = null, int? graduationYear = null, string? role = null)
    {
        var resolvedEmail = !string.IsNullOrWhiteSpace(email)
            ? email
            : $"{name.Trim().ToLower().Replace(" ", ".")}@ogr.iu.edu.tr";

        return new User
        {
            Name = name.Trim(),
            Email = resolvedEmail,
            GraduationYear = graduationYear ?? 2024,
            Role = !string.IsNullOrWhiteSpace(role) ? role : "alumni",
            CreatedAt = DateTime.UtcNow
        };
    }

    public static List<User> CreateDefaultAlumni()
    {
        return new List<User>
        {
            new() { Id = 42, Name = "Elif Kaya", Email = "elif.kaya@ogr.iu.edu.tr", GraduationYear = 2024, Role = "alumni", CreatedAt = new DateTime(2026, 1, 15, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = 43, Name = "Mert Aydın", Email = "mert.aydin@ogr.iu.edu.tr", GraduationYear = 2021, Role = "alumni", CreatedAt = new DateTime(2026, 2, 10, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = 44, Name = "Zeynep Arslan", Email = "zeynep.arslan@ogr.iu.edu.tr", GraduationYear = 2019, Role = "alumni", CreatedAt = new DateTime(2026, 3, 5, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = 45, Name = "Can Öztürk", Email = "can.ozturk@ogr.iu.edu.tr", GraduationYear = 2023, Role = "alumni", CreatedAt = new DateTime(2026, 4, 1, 0, 0, 0, DateTimeKind.Utc) }
        };
    }
}
