using System.ComponentModel.DataAnnotations;

namespace LugenStore.API.DTOs.Auth;

public class RegisterDto : AuthBaseDto
{
    [Required(ErrorMessage = "Password confirmation is required")]
    [Compare("Password", ErrorMessage = "The passwords don't match")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
