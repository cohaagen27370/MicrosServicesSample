using CommonDto.Models;
using CropService;
using Google.Protobuf.WellKnownTypes;


namespace Gateway.Extensions;

public static class CropExtensions
{
    public static CropDto ToCropDto(this Crop crop)
    {
        return new CropDto()
        {
            Id = Guid.Parse(crop.Id),
            SeedId = Guid.Parse(crop.SeedId),
            StartDate = crop.StartDate.ToDateTime(),
            EndDate = crop.EndDate.ToDateTime(),
            Status = (CommonDto.Enumerations.CropStatus)crop.Status
        };
    }
    
    public static Crop ToCrop(this CropDto cropDto)
    {
        return new Crop()
        {
            Id = cropDto.Id.ToString(),
            SeedId = cropDto.SeedId.ToString(),
            StartDate = cropDto.StartDate.ToUniversalTime().ToTimestamp(),
            EndDate = cropDto.EndDate.ToUniversalTime().ToTimestamp(),
            Status = (CropService.CropStatus)cropDto.Status
        };
    }    
    
}