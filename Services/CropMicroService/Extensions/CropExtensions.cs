
using CropMicroService.Entities;
using CropService;
using Google.Protobuf.WellKnownTypes;

namespace CropMicroService.Extensions;

public static class CropExtensions
{
    
    public static CropEntity ToEntity(this Crop crop)
    {
        var newEntity = new CropEntity
        {
            Id = crop.Id != null ? Guid.Parse(crop.Id) : Guid.NewGuid(),
            startDate = crop.StartDate.ToDateTime(),
            endDate = crop.EndDate.ToDateTime(),
            SeedId = Guid.Parse(crop.SeedId),
            Status = crop.Status
        };
    
        return newEntity;
    }
    
    public static Crop ToCrop(this CropEntity cropEntity)
    {
        return new Crop()
        {
            Id = cropEntity.Id.ToString(),
            SeedId = cropEntity.SeedId.ToString(),
            StartDate = cropEntity.startDate.ToUniversalTime().ToTimestamp(),
            EndDate = cropEntity.endDate.ToUniversalTime().ToTimestamp(),
            Status = cropEntity.Status
        };
    }
}