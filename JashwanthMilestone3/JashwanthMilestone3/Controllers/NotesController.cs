using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JashwanthMilestone3.Models;
using JashwanthMilestone3.Services;

namespace JashwanthMilestone3.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class NotesController(AppDataStore store) : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll()
    {
        var userId = GetUserId();
        if (userId is null) return Unauthorized();

        var notes = store.Notes
            .Where(n => n.UserId == userId.Value)
            .Select(n => new { n.Id, n.Title, n.Content })
            .ToList();

        return Ok(notes);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var userId = GetUserId();
        if (userId is null) return Unauthorized();

        var note = store.Notes.FirstOrDefault(n => n.Id == id && n.UserId == userId.Value);
        if (note is null) return NotFound(new { message = "Note not found." });

        return Ok(new { note.Id, note.Title, note.Content });
    }

    [HttpPost]
    public IActionResult Create([FromBody] NoteUpsertRequest request)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var userId = GetUserId();
        if (userId is null) return Unauthorized();

        var note = new Note
        {
            Id = store.NextNoteId(),
            UserId = userId.Value,
            Title = request.Title.Trim(),
            Content = request.Content.Trim()
        };
        store.Notes.Add(note);

        return Ok(new { message = "Note added successfully.", noteId = note.Id });
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, [FromBody] NoteUpsertRequest request)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var userId = GetUserId();
        if (userId is null) return Unauthorized();

        var note = store.Notes.FirstOrDefault(n => n.Id == id && n.UserId == userId.Value);
        if (note is null) return NotFound(new { message = "Note not found." });

        note.Title = request.Title.Trim();
        note.Content = request.Content.Trim();

        return Ok(new { message = "Note updated successfully." });
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var userId = GetUserId();
        if (userId is null) return Unauthorized();

        var note = store.Notes.FirstOrDefault(n => n.Id == id && n.UserId == userId.Value);
        if (note is null) return NotFound(new { message = "Note not found." });

        store.Notes.Remove(note);
        return Ok(new { message = "Note deleted successfully." });
    }

    private int? GetUserId()
    {
        var claim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(claim, out var id) ? id : null;
    }
}
