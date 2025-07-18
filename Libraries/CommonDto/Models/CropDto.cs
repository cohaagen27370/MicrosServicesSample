using CommonDto.Enumerations;

namespace CommonDto.Models;

public class CropDto
{
    public Guid Id { get; set; }

    public Guid SeedId { get; set; }
    
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public CropStatus Status { get; set; }
}