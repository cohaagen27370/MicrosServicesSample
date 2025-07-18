using Microsoft.EntityFrameworkCore;
using SeedMicroService.Entities;

namespace SeedMicroService.Context;

public class SeedContext(DbContextOptions<SeedContext> options) : DbContext(options)
{
    public DbSet<SeedEntity> Seeds { get; set; } = null!;

    public DbSet<CategoryEntity> Categories { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        const int legumeFeuilleId = 1;
        const int legumeFruitId = 2;
        const int legumeRacineId = 3;
        
        modelBuilder.Entity<CategoryEntity>().HasData(
            new CategoryEntity() { Id = legumeFeuilleId, Name = "Légume feuille" },
            new CategoryEntity { Id = legumeFruitId, Name = "Légume fruit" },
            new CategoryEntity { Id = legumeRacineId, Name = "Légume racine" }
        );
        
        modelBuilder.Entity<SeedEntity>().HasData(
            new SeedEntity { Id = Guid.NewGuid(), Name = "Batavia", Species = "Laitue", RisingTime = 5, DurationBeforeHarvest = 30, CategoryId = legumeFeuilleId },
            new SeedEntity { Id = Guid.NewGuid(), Name = "Saint Pierre", Species = "Tomate", RisingTime = 3, DurationBeforeHarvest = 90, CategoryId = legumeFruitId },
            new SeedEntity { Id = Guid.NewGuid(), Name = "Géante de New York", Species = "Aubergine", RisingTime = 10 , DurationBeforeHarvest = 120, CategoryId = legumeFruitId }
        );
    }
    
}