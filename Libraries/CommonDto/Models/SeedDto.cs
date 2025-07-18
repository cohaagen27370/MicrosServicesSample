using System;

namespace CommonDto.Models;

public class SeedDto
{
    public Guid? Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Specy { get; set; } = string.Empty;
    public int RisingTime { get; set; }
    public int DurationBeforeHarvest { get; set; }
    public string Picture { get; set; } = string.Empty;
    public CategoryDto Category { get; set; } = new();
}