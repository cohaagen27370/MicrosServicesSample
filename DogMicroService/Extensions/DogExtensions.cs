using DogMicroService.Entities;
using DogService;


namespace DogMicroService.Extensions;

public static class DogExtensions
{
    public static DogEntity ToEntity(this Dog dog)
    {
        return new DogEntity
        {
            Id = dog.Id != null ? Guid.Parse(dog.Id) : Guid.NewGuid(),
            Name = dog.Name,
            Breed = dog.Breed,
            Age = dog.Age
        };
    }

    public static Dog? ToDog(this DogEntity? dogEntity)
    {
        if (dogEntity == null)
            return null;
        
        return new Dog()
        {
            Id = dogEntity.Id.ToString(),
            Name = dogEntity.Name,
            Breed = dogEntity.Breed,
            Age = dogEntity.Age
        };
    }
    
    
}