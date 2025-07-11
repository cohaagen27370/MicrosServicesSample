using DogService.Entities;
using Microsoft.EntityFrameworkCore;

namespace DogService.Context;

public class DogContext(DbContextOptions<DogContext> options) : DbContext(options)
{
    public DbSet<DogEntity> Dogs { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DogEntity>().HasData(
            new DogEntity { Id = Guid.NewGuid(), Name = "Buddy", Breed = "Labrador", Age = 3 },
            new DogEntity { Id = Guid.NewGuid(), Name = "Lucy", Breed = "Golden Retriever", Age = 5 },
            new DogEntity { Id = Guid.NewGuid(), Name = "Max", Breed = "German Shepherd", Age = 2 }
        );
    }
    
}