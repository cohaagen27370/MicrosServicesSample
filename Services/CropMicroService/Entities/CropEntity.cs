using System.ComponentModel.DataAnnotations;
using CropService;

namespace CropMicroService.Entities;

public class CropEntity
{
    [Key]
    public Guid Id { get; set; }

    public DateTime startDate { get; set; }
    public DateTime endDate { get; set; }

    public CropStatus Status { get; set; }
    
    public Guid SeedId { get; set; }
}