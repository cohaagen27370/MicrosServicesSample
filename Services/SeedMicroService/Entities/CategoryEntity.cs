using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SeedMicroService.Entities;

public class CategoryEntity
{
    [Key]
    public int Id { get; set; }
    
    [Column(TypeName = "varchar(200)")]
    public string Name { get; set; } = string.Empty;
}