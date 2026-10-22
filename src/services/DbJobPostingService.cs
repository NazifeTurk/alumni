using Alumni.Data;
using Alumni.Models;
using Microsoft.EntityFrameworkCore;

namespace Alumni.Services;

public class DbJobPostingService : IJobPostingService
{
    private readonly AlumniDbContext _context;

    public DbJobPostingService(AlumniDbContext context)
    {
        _context = context;
    }

    public IEnumerable<JobPosting> GetAll()
    {
        return _context.JobPostings
            .Include(j => j.User)
            .OrderByDescending(j => j.CreatedAt)
            .ToList();
    }

    public JobPosting? GetById(int id)
    {
        return _context.JobPostings
            .Include(j => j.User)
            .Include(j => j.Applications)
            .FirstOrDefault(j => j.Id == id);
    }

    public IEnumerable<JobPosting> GetByUserId(int userId)
    {
        return _context.JobPostings
            .Where(j => j.UserId == userId)
            .OrderByDescending(j => j.CreatedAt)
            .ToList();
    }

    public JobPosting? CreateForUser(int userId, CreateJobPostingRequest request)
    {
        var userExists = _context.Users.Any(u => u.Id == userId);
        if (!userExists)
        {
            return null; // Will trigger 404 in controller
        }

        var job = new JobPosting
        {
            UserId = userId,
            Title = request.Title.Trim(),
            Company = request.Company.Trim(),
            Location = !string.IsNullOrWhiteSpace(request.Location) ? request.Location.Trim() : "Remote",
            Description = !string.IsNullOrWhiteSpace(request.Description) ? request.Description.Trim() : request.Title.Trim(),
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        _context.JobPostings.Add(job);
        _context.SaveChanges();
        return job;
    }

    public JobPosting? Update(int id, UpdateJobPostingRequest request)
    {
        var job = _context.JobPostings.Find(id);
        if (job == null) return null;

        job.Title = request.Title.Trim();
        job.Company = request.Company.Trim();
        job.Location = request.Location.Trim();
        job.Description = request.Description.Trim();
        job.IsActive = request.IsActive;

        _context.SaveChanges();
        return job;
    }

    public bool Delete(int id)
    {
        var job = _context.JobPostings.Find(id);
        if (job == null) return false;

        _context.JobPostings.Remove(job);
        _context.SaveChanges();
        return true;
    }

    public (Application? application, string? error, int statusCode) Apply(int jobId, int userId)
    {
        var userExists = _context.Users.Any(u => u.Id == userId);
        if (!userExists)
        {
            return (null, $"User with ID '{userId}' was not found.", 404);
        }

        var jobExists = _context.JobPostings.Any(j => j.Id == jobId);
        if (!jobExists)
        {
            return (null, $"Job posting with ID '{jobId}' was not found.", 404);
        }

        var alreadyApplied = _context.Applications.Any(a => a.UserId == userId && a.JobPostingId == jobId);
        if (alreadyApplied)
        {
            return (null, $"User with ID '{userId}' has already applied to job posting '{jobId}'.", 409);
        }

        var application = new Application
        {
            UserId = userId,
            JobPostingId = jobId,
            Status = "applied",
            CreatedAt = DateTime.UtcNow
        };

        _context.Applications.Add(application);
        _context.SaveChanges();
        return (application, null, 201);
    }

    public IEnumerable<Application> GetApplicationsForJob(int jobId)
    {
        return _context.Applications
            .Include(a => a.User)
            .Where(a => a.JobPostingId == jobId)
            .OrderByDescending(a => a.CreatedAt)
            .ToList();
    }

    public IEnumerable<Application> GetApplicationsForUser(int userId)
    {
        return _context.Applications
            .Include(a => a.JobPosting)
            .Where(a => a.UserId == userId)
            .OrderByDescending(a => a.CreatedAt)
            .ToList();
    }
}
