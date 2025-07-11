namespace CommonDto.Models;

public class OrderDto
{
    public Guid Id { get; set; }

    public DateTime OrderDate { get; set; }

    public List<DogDto> Dogs { get; set; } = [];
}