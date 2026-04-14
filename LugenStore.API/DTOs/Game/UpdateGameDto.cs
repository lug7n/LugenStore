using System.ComponentModel.DataAnnotations;

namespace LugenStore.API.DTOs.Game;

public class UpdateGameDto : GameBaseDto
{
    [Required]
    public Guid Id { get; set; }
}
