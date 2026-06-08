using System.ComponentModel.DataAnnotations;

namespace LugenStore.API.DTOs.Auth;

public class RegisterDto
{
    [Required(ErrorMessage = "User name is required")]
    [StringLength(100, MinimumLength = 3)]
    [RegularExpression(@"^[A-Za-zÀ-ÿ]+(?:[ '-][A-Za-zÀ-ÿ]+)*$",
    ErrorMessage = "Name must contain only letters and valid separators.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Cpf is required")]
    [RegularExpression(@"^\d{11}$")]
    public string Cpf { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [StringLength(150)]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z\d])\S{8,}$",
        ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, number, special character and no spaces")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password confirmation is required")]
    [Compare("Password", ErrorMessage = "The passwords don't match")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
