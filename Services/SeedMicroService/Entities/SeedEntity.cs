using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SeedMicroService.Entities;

public class SeedEntity
{
    [Key]
    public Guid Id { get; set; }
    
    [Column(TypeName = "varchar(200)")]
    public string Name { get; set; } = string.Empty;
    
    [Column(TypeName = "varchar(200)")]    
    public string Species { get; set; } = string.Empty;
    public int RisingTime { get; set; }
    
    public int DurationBeforeHarvest { get; set; }

    public string Picture { get; set; } = string.Empty;
    
    public int CategoryId { get; set; }
    public CategoryEntity Category { get; set; } = null!;
    
}