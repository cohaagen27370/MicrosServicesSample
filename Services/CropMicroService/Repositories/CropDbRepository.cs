using CropMicroService.Context;
using CropMicroService.Entities;
using Microsoft.EntityFrameworkCore;

namespace CropMicroService.Repositories;

public class CropDbRepository(CropContext context) : ICropDbRepository
{
    public async Task<Guid> AddCropAsync(CropEntity crop)
    {
        await context.Crops.AddAsync(crop);
        await context.SaveChangesAsync();

        return crop.Id;
    }

    public async Task<CropEntity?> GetCropByIdAsync(Guid id)
    {
        return await context.Crops.FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task<List<CropEntity>> GetAllCropsAsync()
    {
        return await context.Crops.ToListAsync();
    }
    
    public async Task UpdateCropAsync(CropEntity crop)
    {
        context.Crops.Update(crop);
        await context.SaveChangesAsync();
    }

    public async Task DeleteCropAsync(Guid id)
    {
        CropEntity? cropToDelete = await context.Crops.FirstOrDefaultAsync(d => d.Id == id);
        if (cropToDelete != null)
        {
            context.Crops.Remove(cropToDelete);
            await context.SaveChangesAsync();
        }
    }

    
}