using LugenStore.API.DTOs.Auth;
using LugenStore.API.Exceptions;
using LugenStore.API.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace LugenStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService _service) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody]RegisterDto dto)
    {
        try
        {
            var user = await _service.RegisterAsync(dto);
            return CreatedAtAction(
                actionName: "GetById",
                controllerName: "User",
                routeValues: new { id = user.Id },
                value: user
            );
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody]LoginDto dto)
    {
        try
        {
            var result  = await _service.LoginAsync(dto);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
    }
}
