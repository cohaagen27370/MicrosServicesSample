using CommonDto.Models;

namespace Front.Services;

public interface IDataService
{
    Task<List<DogDto>?> GetAllDogs();
}