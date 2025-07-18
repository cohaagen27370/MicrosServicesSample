using Microsoft.EntityFrameworkCore;
using SeedMicroService.Context;
using SeedMicroService.Entities;

namespace SeedMicroService.Repositories;

public class SeedDbRepository(SeedContext context) : ISeedDbRepository
{
    public async Task<Guid> AddSeedAsync(SeedEntity seed)
    {
        await context.Seeds.AddAsync(seed);
        await context.SaveChangesAsync();

        return seed.Id;
    }

    public async Task<SeedEntity?> GetSeedByIdAsync(Guid id)
    {
        return await context.Seeds.FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task<List<SeedEntity>> GetAllSeedsAsync()
    {
        return await context.Seeds.ToListAsync();
    }

    public async Task<List<CategoryEntity>> GetAllCategoriesAsync()
    {
        return await context.Categories.ToListAsync();
    }

    public async Task<CategoryEntity?> GetCategoryByIdAsync(int id)
    {
        return await context.Categories.FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task UpdateSeedAsync(SeedEntity seed)
    {
        context.Seeds.Update(seed);
        await context.SaveChangesAsync();
    }

    public async Task DeleteSeedAsync(Guid id)
    {
        SeedEntity? dogToDelete = await context.Seeds.FirstOrDefaultAsync(d => d.Id == id);
        if (dogToDelete != null)
        {
            context.Seeds.Remove(dogToDelete);
            await context.SaveChangesAsync();
        }
    }

    
}