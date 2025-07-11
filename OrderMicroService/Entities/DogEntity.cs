using System.ComponentModel.DataAnnotations;

namespace OrderMicroService.Entities;

public class DogEntity
{
    [Key]
    public Guid Id { get; set; }

    public Guid IdFromDogService { get; set; }

    public string Name { get; set; } = string.Empty;
}