using SeedMicroService.Entities;

namespace SeedMicroService.Repositories;

public interface ISeedDbRepository
{
    Task<Guid> AddSeedAsync(SeedEntity seed);
    Task<SeedEntity?> GetSeedByIdAsync(Guid id);
    Task<List<SeedEntity>> GetAllSeedsAsync();
    Task<List<CategoryEntity>> GetAllCategoriesAsync();
    Task<CategoryEntity?> GetCategoryByIdAsync(int id);
    Task UpdateSeedAsync(SeedEntity seed);
    Task DeleteSeedAsync(Guid id);
}