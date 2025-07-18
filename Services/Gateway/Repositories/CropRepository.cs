using Google.Protobuf.Collections;
using CropService;

namespace Gateway.Repositories;

public class CropRepository(CropService.CropGrpcService.CropGrpcServiceClient grpcClient) : ICropRepository
{
    public async Task<Guid> AddCrop(Crop newCrop)
    {
        var request = new AddCropRequest() { Crop = newCrop };
        CropResponse? response = await grpcClient.AddCropAsync(request);
        return Guid.Parse(response.Id);
    }

    public async Task<Guid> UpdateCrop(Guid cropId, Crop cropToUpdate)
    {
        var request = new UpdateCropRequest() { CropId = cropId.ToString(), Crop = cropToUpdate};
        CropResponse? response = await grpcClient.UpdateCropAsync(request);
        return Guid.Parse(response.Id);
    }

    public async Task<RepeatedField<Crop>> GetAllCrops()
    {
        var request = new ListCropsRequest();
        ListCropsResponse? response = await grpcClient.ListCropsAsync(request);
        return response.Crops;
    }
    
}