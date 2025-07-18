using Google.Protobuf.Collections;
using CropService;

namespace Gateway.Repositories;

public interface ICropRepository
{
    Task<Guid> AddCrop(Crop newCrop);

    Task<Guid> UpdateCrop(Guid cropId, Crop cropToUpdate);

    Task<RepeatedField<Crop>> GetAllCrops();
}