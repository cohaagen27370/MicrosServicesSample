using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderMicroService.Entities;

public class OrderEntity
{
    [Key]
    public Guid Id { get; set; }

    public DateTime OrderDate { get; set; }

    public List<DogEntity> Dogs { get; set; } = [];
}