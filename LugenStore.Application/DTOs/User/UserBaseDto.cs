using System.ComponentModel.DataAnnotations;

namespace LugenStore.Application.DTOs.User;

public class UserBaseDto
{
    [Required(ErrorMessage = "User name is required")]
    [StringLength(100, MinimumLength = 3)]
    [RegularExpression(@"^[A-Za-zÀ-ÿ]+(?:[ '-][A-Za-zÀ-ÿ]+)*$",
    ErrorMessage = "Name must contain only letters and valid separators.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "User email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [StringLength(150)]
    public string Email { get; set; } = string.Empty;
}
