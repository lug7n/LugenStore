using Microsoft.EntityFrameworkCore.Diagnostics;
using System.ComponentModel.DataAnnotations;

namespace LugenStore.API.DTOs.Publisher;

public class PublisherBaseDto
{
    [StringLength(30, MinimumLength = 2)]
    public string Name { get; set; } = null!;
}
