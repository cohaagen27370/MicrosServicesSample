using CropMicroService.Entities;

namespace CropMicroService.Repositories;

public interface ICropDbRepository
{
    Task<Guid> AddCropAsync(CropEntity crop);
    Task<CropEntity?> GetCropByIdAsync(Guid id);

    Task<List<CropEntity>> GetAllCropsAsync();

    Task UpdateCropAsync(CropEntity crop);

    Task DeleteCropAsync(Guid id);
}