using Alumni.Models;

namespace Alumni.Services;

public interface IJobPostingService
{
    IEnumerable<JobPosting> GetAll();
    JobPosting? GetById(int id);
    IEnumerable<JobPosting> GetByUserId(int userId);
    JobPosting? CreateForUser(int userId, CreateJobPostingRequest request);
    JobPosting? Update(int id, UpdateJobPostingRequest request);
    bool Delete(int id);

    // Application Many-to-Many methods
    (Application? application, string? error, int statusCode) Apply(int jobId, int userId);
    IEnumerable<Application> GetApplicationsForJob(int jobId);
    IEnumerable<Application> GetApplicationsForUser(int userId);
}
