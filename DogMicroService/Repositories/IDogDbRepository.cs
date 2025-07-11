using DogService.Entities;

namespace DogService.Repositories;

public interface IDogDbRepository
{
    Task<Guid> AddDogAsync(DogEntity dog);
    Task<DogEntity?> GetDogByIdAsync(Guid id);
    Task<List<DogEntity>> GetAllDogsAsync();
    Task UpdateDogAsync(DogEntity dog);
    Task DeleteDogAsync(Guid id);
}