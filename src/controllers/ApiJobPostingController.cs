using Alumni.Models;
using Alumni.Services;
using Microsoft.AspNetCore.Mvc;

namespace Alumni.Controllers;

[ApiController]
[Produces("application/json")]
[Tags("Job Postings & Applications API (Week 06 Milestone)")]
public class ApiJobPostingController : ControllerBase
{
    private readonly IJobPostingService _jobService;

    public ApiJobPostingController(IJobPostingService jobService)
    {
        _jobService = jobService;
    }

    // 🌟 Slide Section B: The Nested Route — POST /api/users/{id}/jobs
    [HttpPost("api/users/{id:int}/jobs")]
    [ProducesResponseType(typeof(JobPosting), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult CreateUserJob(int id, [FromBody] CreateJobPostingRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title) || string.IsNullOrWhiteSpace(request.Company))
        {
            return BadRequest(new { error = 400, message = "Title and Company are required." });
        }

        var created = _jobService.CreateForUser(id, request);
        if (created == null)
        {
            // Slide Section B: does user 1 exist? No -> 404 user not found
            return NotFound(new { error = 404, message = $"User with ID '{id}' was not found." });
        }

        // Slide Section B: 201, and the new row carries user_id = id
        return Created($"/api/jobs/{created.Id}", created);
    }

    // GET /api/users/{id}/jobs -> List all jobs posted by specific user
    [HttpGet("api/users/{id:int}/jobs")]
    [ProducesResponseType(typeof(IEnumerable<JobPosting>), StatusCodes.Status200OK)]
    public IActionResult GetUserJobs(int id)
    {
        var jobs = _jobService.GetByUserId(id);
        return Ok(jobs);
    }

    // GET /api/jobs -> List all jobs
    [HttpGet("api/jobs")]
    [ProducesResponseType(typeof(IEnumerable<JobPosting>), StatusCodes.Status200OK)]
    public IActionResult GetAll()
    {
        var jobs = _jobService.GetAll();
        return Ok(jobs);
    }

    // GET /api/jobs/{id} -> Single job by ID
    [HttpGet("api/jobs/{id:int}")]
    [ProducesResponseType(typeof(JobPosting), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(int id)
    {
        var job = _jobService.GetById(id);
        if (job == null)
        {
            return NotFound(new { error = 404, message = $"Job posting with ID '{id}' was not found." });
        }

        return Ok(job);
    }

    // PUT /api/jobs/{id} -> Update job
    [HttpPut("api/jobs/{id:int}")]
    [ProducesResponseType(typeof(JobPosting), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(int id, [FromBody] UpdateJobPostingRequest request)
    {
        var updated = _jobService.Update(id, request);
        if (updated == null)
        {
            return NotFound(new { error = 404, message = $"Job posting with ID '{id}' was not found." });
        }

        return Ok(updated);
    }

    // DELETE /api/jobs/{id} -> Delete job
    [HttpDelete("api/jobs/{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id)
    {
        var deleted = _jobService.Delete(id);
        if (!deleted)
        {
            return NotFound(new { error = 404, message = $"Job posting with ID '{id}' was not found." });
        }

        return Ok(new { status = "success", message = $"Job posting with ID '{id}' was deleted." });
    }

    // 🌟 Slide Section A: Many-to-Many Application — POST /api/jobs/{id}/applications
    [HttpPost("api/jobs/{id:int}/applications")]
    [ProducesResponseType(typeof(Application), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public IActionResult ApplyToJob(int id, [FromBody] CreateApplicationRequest request)
    {
        var (application, error, statusCode) = _jobService.Apply(id, request.UserId);

        if (statusCode == 404)
        {
            return NotFound(new { error = 404, message = error });
        }

        if (statusCode == 409)
        {
            // Slide Section A: UNIQUE (user_id, job_posting_id) -> twice: 409
            return Conflict(new { error = 409, message = error });
        }

        return Created($"/api/jobs/{id}/applications/{application?.Id}", application);
    }

    // GET /api/jobs/{id}/applications -> List all applicants for a job posting
    [HttpGet("api/jobs/{id:int}/applications")]
    [ProducesResponseType(typeof(IEnumerable<Application>), StatusCodes.Status200OK)]
    public IActionResult GetJobApplications(int id)
    {
        var applications = _jobService.GetApplicationsForJob(id);
        return Ok(applications);
    }

    // GET /api/users/{id}/applications -> List all applications submitted by a user
    [HttpGet("api/users/{id:int}/applications")]
    [ProducesResponseType(typeof(IEnumerable<Application>), StatusCodes.Status200OK)]
    public IActionResult GetUserApplications(int id)
    {
        var applications = _jobService.GetApplicationsForUser(id);
        return Ok(applications);
    }
}
