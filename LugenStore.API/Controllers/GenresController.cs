using LugenStore.API.DTOs.Genre;
using LugenStore.API.Exceptions;
using LugenStore.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LugenStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GenresController : ControllerBase
{
    private readonly IGenreService _service;

    public GenresController(IGenreService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var genres = await _service.GetAllAsync();
        return Ok(genres);
    }

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

    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateGenreDto dto)
    {
        try
        {
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
