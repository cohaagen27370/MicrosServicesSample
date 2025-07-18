using SeedService;
using SeedMicroService.Entities;


namespace SeedMicroService.Extensions;

public static class SeedExtensions
{
    public static SeedEntity ToEntity(this Seed seed)
    {
        return new SeedEntity
        {
            Id = seed.Id != null ? Guid.Parse(seed.Id) : Guid.NewGuid(),
            Name = seed.Name,
            Species = seed.Specy,
            RisingTime = seed.RisingTime,
            DurationBeforeHarvest = seed.DurationBeforeHarvest,
            Picture = seed.Picture,
            CategoryId = seed.CategoryId
        };
    }

    public static Seed? ToSeed(this SeedEntity? seedEntity)
    {
        if (seedEntity == null)
            return null;
        
        return new Seed()
        {
            Id = seedEntity.Id.ToString(),
            Name = seedEntity.Name,
            Specy = seedEntity.Species,
            RisingTime = seedEntity.RisingTime,
            DurationBeforeHarvest = seedEntity.DurationBeforeHarvest,
            Picture = seedEntity.Picture,
            CategoryId = seedEntity.CategoryId
        };
    }
    
    public static Category? ToCategory(this CategoryEntity? categoryEntity)
    {
        if (categoryEntity == null)
            return null;
        
        return new Category()
        {
            Id = categoryEntity.Id,
            Name = categoryEntity.Name
        };
    }    
    
    
    
    
}