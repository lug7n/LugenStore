using LugenStore.Application.DTOs.Genre;
using LugenStore.Application.Interfaces;
using LugenStore.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LugenStore.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class GenresController(IGenreService _service) : ControllerBase
{
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var genres = await _service.GetAllAsync();
        return Ok(genres);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        try
        {
            var genre = await _service.GetByIdAsync(id);
            return Ok(genre);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateGenreDto dto)
    {
        try
        {
            var genre = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = genre.Id }, genre);
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }

    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateGenreDto dto)
    {
        try
        {
            if (id != dto.Id)
                return BadRequest("Route id and body id must match");

            await _service.UpdateAsync(dto);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        try
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
