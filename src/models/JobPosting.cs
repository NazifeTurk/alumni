using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Alumni.Models;

public class JobPosting
{
    public int Id { get; set; }
    public int UserId { get; set; }

    [Required(ErrorMessage = "Title is required.")]
    [StringLength(120, ErrorMessage = "Title cannot exceed 120 characters.")]
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Company is required.")]
    [StringLength(100, ErrorMessage = "Company cannot exceed 100 characters.")]
    [JsonPropertyName("company")]
    public string Company { get; set; } = string.Empty;

    [Required(ErrorMessage = "City is required.")]
    [StringLength(100, ErrorMessage = "City cannot exceed 100 characters.")]
    [JsonPropertyName("city")]
    public string City { get; set; } = "Istanbul";

    [JsonPropertyName("location")]
    public string Location
    {
        get => string.IsNullOrWhiteSpace(City) ? _location : City;
        set
        {
            _location = value;
            if (string.IsNullOrWhiteSpace(City)) City = value;
        }
    }
    private string _location = "Remote";

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [JsonPropertyName("isActive")]
    public bool IsActive { get; set; } = true;

    // Navigation Properties
    [JsonIgnore]
    public User? User { get; set; }

    [JsonIgnore]
    public ICollection<Application> Applications { get; set; } = new List<Application>();
}

public record CreateJobPostingRequest
{
    [Required(ErrorMessage = "Title is required.")]
    [StringLength(120, ErrorMessage = "Title cannot exceed 120 characters.")]
    public string Title { get; init; } = string.Empty;

    [Required(ErrorMessage = "Company is required.")]
    public string Company { get; init; } = string.Empty;

    public string? City { get; init; }
    public string? Location { get; init; }
    public string? Description { get; init; }

    [JsonConstructor]
    public CreateJobPostingRequest(string title, string company, string? city = null, string? location = null, string? description = null)
    {
        Title = title;
        Company = company;
        City = city;
        Location = location;
        Description = description;
    }

    [JsonIgnore]
    public string EffectiveCity => !string.IsNullOrWhiteSpace(City) ? City : (!string.IsNullOrWhiteSpace(Location) ? Location : string.Empty);
}

public record UpdateJobPostingRequest
{
    [Required(ErrorMessage = "Title is required.")]
    [StringLength(120, ErrorMessage = "Title cannot exceed 120 characters.")]
    public string Title { get; init; } = string.Empty;

    [Required(ErrorMessage = "Company is required.")]
    public string Company { get; init; } = string.Empty;

    public string? City { get; init; }
    public string? Location { get; init; }
    public string? Description { get; init; }
    public bool IsActive { get; init; } = true;

    [JsonConstructor]
    public UpdateJobPostingRequest(string title, string company, string? city = null, string? location = null, string? description = null, bool isActive = true)
    {
        Title = title;
        Company = company;
        City = city;
        Location = location;
        Description = description;
        IsActive = isActive;
    }

    public UpdateJobPostingRequest(string title, string company, string location, string description, bool isActive)
        : this(title, company, location, location, description, isActive) {}

    [JsonIgnore]
    public string EffectiveCity => !string.IsNullOrWhiteSpace(City) ? City : (!string.IsNullOrWhiteSpace(Location) ? Location : string.Empty);
}
