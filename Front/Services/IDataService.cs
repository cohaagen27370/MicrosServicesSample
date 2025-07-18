using CommonDto.Models;

namespace Front.Services;

public interface IDataService
{
    
    Task<Guid> ModifySeed(SeedDto seed);
    Task DeleteSeed(Guid? seedId);
    Task<List<SeedDto>?> GetAllSeeds();

    Task<Guid> AddNewCrop(CropDto crop);
    Task<List<CropDto>?> GetAllCrops();
    Task<SeedDto?> GetSeedById(Guid? seedId);

}