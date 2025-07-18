using Google.Protobuf.Collections;
using SeedService;

namespace Gateway.Repositories;

public interface ISeedRepository
{
    Task<Seed> GetOneSeedAsync(Guid id);
    Task<Category> GetOneCategoryAsync(int id);
    Task<RepeatedField<Seed>> GetAllSeedsAsync();
    Task<Guid> AddSeed(Seed newSeed);
    Task<Guid> UpdateSeed(Guid seedId,Seed updatedSeed);
    Task DeleteSeed(Guid seedId);
}