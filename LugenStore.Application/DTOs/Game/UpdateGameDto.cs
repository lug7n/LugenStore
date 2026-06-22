using System.ComponentModel.DataAnnotations;

namespace LugenStore.Application.DTOs.Game;

public class UpdateGameDto : GameBaseDto
{
    [Required]
    public Guid Id { get; set; }
}
