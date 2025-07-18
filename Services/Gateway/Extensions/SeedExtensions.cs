using CommonDto.Models;
using SeedService;

namespace Gateway.Extensions;

public static class SeedExtensions
{

    public static Seed ToSeed(this SeedDto seedDto)
    {
        return new Seed() {
            Id = seedDto.Id == null ? Guid.NewGuid().ToString() : seedDto.Id.ToString(),
            Name = seedDto.Name,
            Specy = seedDto.Specy,
            RisingTime = seedDto.RisingTime,
            DurationBeforeHarvest = seedDto.DurationBeforeHarvest,
            Picture = seedDto.Picture,
            CategoryId = seedDto.Category.Id
        };   
    }
    
    
    public static SeedDto ToSeedDto(this Seed seed)
    {
        return new SeedDto()
        {
            Id = Guid.Parse(seed.Id),
            Name = seed.Name,
            Specy = seed.Specy,
            RisingTime = seed.RisingTime,
            DurationBeforeHarvest = seed.DurationBeforeHarvest,
            Picture = seed.Picture,
            Category = new CategoryDto() { Id = seed.CategoryId }
        };
    }

    public static CategoryDto ToCategoryDto(this Category category)
    {
        return new CategoryDto()
        {
            Id = category.Id,
            Name = category.Name
        };   
    }
    
}