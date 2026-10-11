namespace Alumni.Routes;

public static class AlumniApiRoutes
{
    // Seed sample data for demonstration of RESTful resource routing
    private static readonly List<AlumnusRecord> SeedAlumni = new()
    {
        new AlumnusRecord(
            Id: 42,
            Name: "Emre Akadal",
            Faculty: "Computer Science",
            GraduationYear: 2015,
            CurrentRole: "Senior Software Architect",
            Company: "CloudScale Technologies",
            Jobs: new List<JobHistoryRecord>
            {
                new(1, "CloudScale Technologies", "Senior Software Architect", 2021, null),
                new(2, "InnoTech Solutions", "Backend Developer", 2016, 2021),
                new(3, "StartupLab", "Junior Software Engineer", 2015, 2016)
            }
        ),
        new AlumnusRecord(
            Id: 101,
            Name: "Ada Lovelace",
            Faculty: "Mathematics & Computing",
            GraduationYear: 2012,
            CurrentRole: "Lead Algorithm Engineer",
            Company: "Quantum Analytics",
            Jobs: new List<JobHistoryRecord>
            {
                new(4, "Quantum Analytics", "Lead Algorithm Engineer", 2018, null),
                new(5, "DataCorp", "Systems Analyst", 2012, 2018)
            }
        )
    };

    public static void MapAlumniApiRoutes(this WebApplication app)
    {
        var group = app.MapGroup("/api/alumni");

        // GET /api/alumni -> List all alumni resources (Status: 200 OK)
        group.MapGet("/", (string? sort) =>
        {
            var result = SeedAlumni.AsEnumerable();
            if (string.Equals(sort, "year", StringComparison.OrdinalIgnoreCase))
            {
                result = result.OrderBy(a => a.GraduationYear);
            }
            return Results.Ok(result.Select(a => new
            {
                a.Id,
                a.Name,
                a.Faculty,
                a.GraduationYear,
                a.CurrentRole,
                a.Company,
                links = new
                {
                    self = $"/api/alumni/{a.Id}",
                    jobs = $"/api/alumni/{a.Id}/jobs"
                }
            }));
        });

        // GET /api/alumni/{id} -> Dynamic nested wildcard route (Status: 200 OK or 404 Not Found)
        group.MapGet("/{id:int}", (int id) =>
        {
            var alumnus = SeedAlumni.FirstOrDefault(a => a.Id == id);
            if (alumnus == null)
            {
                return Results.NotFound(new
                {
                    status = "error",
                    code = 404,
                    message = $"Alumnus resource with ID '{id}' was not found.",
                    timestamp = DateTime.UtcNow
                });
            }

            return Results.Ok(new
            {
                alumnus.Id,
                alumnus.Name,
                alumnus.Faculty,
                alumnus.GraduationYear,
                alumnus.CurrentRole,
                alumnus.Company,
                links = new
                {
                    self = $"/api/alumni/{alumnus.Id}",
                    jobs = $"/api/alumni/{alumnus.Id}/jobs"
                }
            });
        });

        // GET /api/alumni/{id}/jobs -> Dynamic nested route: Jobs (Status: 200 OK or 404 Not Found)
        group.MapGet("/{id:int}/jobs", (int id) =>
        {
            var alumnus = SeedAlumni.FirstOrDefault(a => a.Id == id);
            if (alumnus == null)
            {
                return Results.NotFound(new
                {
                    status = "error",
                    code = 404,
                    message = $"Cannot retrieve jobs. Alumnus with ID '{id}' was not found.",
                    timestamp = DateTime.UtcNow
                });
            }

            return Results.Ok(new
            {
                alumnusId = alumnus.Id,
                alumnusName = alumnus.Name,
                totalJobs = alumnus.Jobs.Count,
                jobs = alumnus.Jobs
            });
        });

        // POST /api/alumni -> Create resource (Status: 201 Created)
        group.MapPost("/", (CreateAlumnusDto dto) =>
        {
            if (string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.Faculty))
            {
                return Results.BadRequest(new
                {
                    status = "error",
                    code = 400,
                    message = "Name and Faculty are required fields."
                });
            }

            int newId = SeedAlumni.Max(a => a.Id) + 1;
            var created = new AlumnusRecord(
                Id: newId,
                Name: dto.Name,
                Faculty: dto.Faculty,
                GraduationYear: dto.GraduationYear > 0 ? dto.GraduationYear : DateTime.UtcNow.Year,
                CurrentRole: dto.CurrentRole ?? "Alumnus",
                Company: dto.Company ?? "Independent",
                Jobs: new List<JobHistoryRecord>()
            );

            SeedAlumni.Add(created);

            return Results.Created($"/api/alumni/{newId}", new
            {
                status = "success",
                code = 201,
                message = "Alumnus resource successfully created.",
                resource = created
            });
        });
    }

    public record AlumnusRecord(
        int Id,
        string Name,
        string Faculty,
        int GraduationYear,
        string CurrentRole,
        string Company,
        List<JobHistoryRecord> Jobs
    );

    public record JobHistoryRecord(
        int Id,
        string Company,
        string Role,
        int StartYear,
        int? EndYear
    );

    public record CreateAlumnusDto(
        string Name,
        string Faculty,
        int GraduationYear,
        string? CurrentRole,
        string? Company
    );
}
