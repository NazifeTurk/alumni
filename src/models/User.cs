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
