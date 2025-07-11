using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DogMicroService.Entities;

public class DogEntity
{
    [Key]
    public Guid Id { get; set; }
    
    [Column(TypeName = "varchar(200)")]
    public string Name { get; set; } = string.Empty;
    
    [Column(TypeName = "varchar(200)")]    
    public string Breed { get; set; } = string.Empty;
    public int Age { get; set; }
}