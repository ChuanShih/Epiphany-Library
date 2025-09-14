using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using EpiphanyLibrary.Api.Application.DTOs;
using EpiphanyLibrary.Api.Application.Interfaces;

namespace EpiphanyLibrary.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookNotesController : ControllerBase
{
    private readonly IBookNoteService _bookNoteService;

    public BookNotesController(IBookNoteService bookNoteService)
    {
        _bookNoteService = bookNoteService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookNoteDto>>> GetAll()
    {
        var bookNotes = await _bookNoteService.GetAllAsync();
        return Ok(bookNotes);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BookNoteDto>> GetById(string id)
    {
        var bookNote = await _bookNoteService.GetByIdAsync(id);
        if (bookNote == null)
        {
            return NotFound();
        }
        return Ok(bookNote);
    }

    [HttpGet("author/{authorId}")]
    public async Task<ActionResult<IEnumerable<BookNoteDto>>> GetByAuthorId(string authorId)
    {
        var bookNotes = await _bookNoteService.GetByAuthorIdAsync(authorId);
        return Ok(bookNotes);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<BookNoteDto>> Create(CreateBookNoteDto createDto)
    {
        var userId = GetCurrentUserId();
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        try
        {
            var bookNote = await _bookNoteService.CreateAsync(createDto, userId);
            return CreatedAtAction(nameof(GetById), new { id = bookNote.Id }, bookNote);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult<BookNoteDto>> Update(string id, UpdateBookNoteDto updateDto)
    {
        var userId = GetCurrentUserId();
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        try
        {
            var bookNote = await _bookNoteService.UpdateAsync(id, updateDto, userId);
            if (bookNote == null)
            {
                return NotFound();
            }
            return Ok(bookNote);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<ActionResult> Delete(string id)
    {
        var userId = GetCurrentUserId();
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var result = await _bookNoteService.DeleteAsync(id, userId);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }

    private string? GetCurrentUserId()
    {
        return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}
