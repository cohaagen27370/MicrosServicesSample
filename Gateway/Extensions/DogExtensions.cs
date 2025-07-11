using CommonDto.Models;
using DogService;
using OrderService;

namespace Gateway.Extensions;

public static class DogExtensions
{

    public static DogDto ToDogDto(this Dog dog)
    {
        return new DogDto()
        {
            Id = Guid.Parse(dog.Id),
            Name = dog.Name,
            Breed = dog.Breed,
            Age = dog.Age
        };
    }
    
    public static DogDto ToOrderedDogDto(this OrderedDog dog)
    {
        return new DogDto()
        {
            Id = Guid.Parse(dog.Id),
            Name = dog.Name
        };
    }
    
}