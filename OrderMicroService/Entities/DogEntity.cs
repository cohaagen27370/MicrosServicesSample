using System.ComponentModel.DataAnnotations;

namespace OrderService.Entities;

public class DogEntity
{
    [Key]
    public Guid Id { get; set; }

    public Guid IdFromDogService { get; set; }

    public string Name { get; set; } = string.Empty;
}