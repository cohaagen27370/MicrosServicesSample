using DogService;
using Google.Protobuf.Collections;

namespace Gateway.Repositories;

public interface IDogRepository
{
    Dog GetOneDogAsync(Guid id);
    Task<RepeatedField<Dog>> GetAllDogsAsync();
    Task<Guid> AddDog(Dog newDog);
    Task<Guid> UpdateDog(Guid dogId, Dog updatedDog);
    Task DeleteDog(Guid dogId);
}