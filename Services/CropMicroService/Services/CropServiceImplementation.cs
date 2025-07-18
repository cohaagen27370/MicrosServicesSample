using CropMicroService.Context;
using CropMicroService.Extensions;
using CropMicroService.Repositories;
using CropService;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.EntityFrameworkCore;
using SeedService;

namespace CropMicroService.Services;

public class CropServiceImplementation(ILogger<CropServiceImplementation> logger, ICropDbRepository cropDbRepository) : CropGrpcService.CropGrpcServiceBase
{

   public override async Task<CropResponse> AddCrop(AddCropRequest request, ServerCallContext context1)
    {
        try 
        {
            Guid id = await cropDbRepository.AddCropAsync(request.Crop.ToEntity());
            
            return new CropResponse() { Success = true, Id = id.ToString() };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during adding seed");
            return new CropResponse() { Success = false, Error = ex.Message };
        }
    }

    public override async Task<CropResponse> UpdateCrop(UpdateCropRequest request, ServerCallContext context1)
    {
        try
        {
            request.Crop.Id = request.CropId;
            await cropDbRepository.UpdateCropAsync(request.Crop.ToEntity());
            return new CropResponse() { Success = true, Id = request.CropId };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during updating dog");            
            return new CropResponse() { Success = false, Error = ex.Message };
        }
    }

    public override async Task<ListCropsResponse>  ListCrops(ListCropsRequest request, ServerCallContext context1)
    {
        try
        {
            var result = new ListCropsResponse();
            result.Crops.AddRange((await cropDbRepository.GetAllCropsAsync()).Select(x => x.ToCrop()));
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during listing dogs");            
            return new ListCropsResponse();
        }
    }
    
}