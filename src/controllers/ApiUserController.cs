using Alumni.Models;
using Alumni.Services;
using Microsoft.AspNetCore.Mvc;
using UserModel = Alumni.Models.User;

namespace Alumni.Controllers;

[ApiController]
[Route("api/users")]
[Produces("application/json")]
[Tags("Users API (ApiUserController)")]
public class ApiUserController : ControllerBase
{
    private readonly IUserService _userService;

    public ApiUserController(IUserService userService)
    {
        _userService = userService;
    }

    // GET /api/users -> List all users (JSON)
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UserModel>), StatusCodes.Status200OK)]
    public IActionResult GetAll()
    {
        return Ok(_userService.GetAll());
    }

    // GET /api/users/{id} -> Get single user by ID (JSON)
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(UserModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(int id)
    {
        var user = _userService.GetById(id);
        if (user == null)
        {
            return NotFound(new { error = 404, message = $"User with ID '{id}' was not found." });
        }

        return Ok(user);
    }

    // POST /api/users -> Create new user (JSON)
    [HttpPost]
    [ProducesResponseType(typeof(UserModel), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] CreateUserRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest(new { error = 400, message = "Name is required." });
        }

        var created = _userService.Create(request);
        return Created($"/api/users/{created.Id}", created);
    }

    // PUT /api/users/{id} -> Replace entire user (JSON)
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(UserModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(int id, [FromBody] UpdateUserRequest request)
    {
        var updated = _userService.Update(id, request);
        if (updated == null)
        {
            return NotFound(new { error = 404, message = $"User with ID '{id}' was not found." });
        }

        return Ok(updated);
    }

    // PATCH /api/users/{id} -> Partial update user (JSON)
    [HttpPatch("{id:int}")]
    [ProducesResponseType(typeof(UserModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Patch(int id, [FromBody] PatchUserRequest request)
    {
        var patched = _userService.Patch(id, request);
        if (patched == null)
        {
            return NotFound(new { error = 404, message = $"User with ID '{id}' was not found." });
        }

        return Ok(patched);
    }

    // DELETE /api/users/{id} -> Remove user (JSON)
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id)
    {
        var deleted = _userService.Delete(id);
        if (!deleted)
        {
            return NotFound(new { error = 404, message = $"User with ID '{id}' was not found." });
        }

        return Ok(new { status = "success", message = $"User with ID '{id}' was deleted." });
    }
}
