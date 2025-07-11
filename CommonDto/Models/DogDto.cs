using System;

namespace CommonDto.Models;

public class DogDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Breed { get; set; } = string.Empty;

    public int? Age { get; set; }
}