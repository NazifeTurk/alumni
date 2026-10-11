using Alumni.Models;
using Alumni.Services;
using Microsoft.AspNetCore.Mvc;

namespace Alumni.Controllers;

[ApiController]
[Route("api/announcements")]
[Produces("application/json")]
[Tags("Announcements API (Week 04 Homework)")]
public class ApiAnnouncementController : ControllerBase
{
    private readonly IAnnouncementService _announcementService;

    public ApiAnnouncementController(IAnnouncementService announcementService)
    {
        _announcementService = announcementService;
    }

    // GET /api/announcements -> List all announcements (JSON)
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Announcement>), StatusCodes.Status200OK)]
    public IActionResult GetAll()
    {
        var announcements = _announcementService.GetAll();
        return Ok(announcements);
    }

    // GET /api/announcements/{id} -> Single announcement by ID (JSON)
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Announcement), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(int id)
    {
        var announcement = _announcementService.GetById(id);
        if (announcement == null)
        {
            return NotFound(new { error = 404, message = $"Announcement with ID '{id}' was not found." });
        }

        return Ok(announcement);
    }

    // POST /api/announcements -> Create announcement (JSON)
    [HttpPost]
    [ProducesResponseType(typeof(Announcement), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] CreateAnnouncementRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title) || string.IsNullOrWhiteSpace(request.Content))
        {
            return BadRequest(new { error = 400, message = "Title and Content are required." });
        }

        var created = _announcementService.Create(request);
        return Created($"/api/announcements/{created.Id}", created);
    }

    // PUT /api/announcements/{id} -> Update announcement (JSON)
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(Announcement), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(int id, [FromBody] UpdateAnnouncementRequest request)
    {
        var updated = _announcementService.Update(id, request);
        if (updated == null)
        {
            return NotFound(new { error = 404, message = $"Announcement with ID '{id}' was not found." });
        }

        return Ok(updated);
    }

    // DELETE /api/announcements/{id} -> Remove announcement (JSON)
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id)
    {
        bool deleted = _announcementService.Delete(id);
        if (!deleted)
        {
            return NotFound(new { error = 404, message = $"Announcement with ID '{id}' was not found." });
        }

        return Ok(new { status = "success", message = $"Announcement with ID '{id}' was deleted." });
    }
}
