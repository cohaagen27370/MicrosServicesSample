using DogMicroService.Context;
using DogMicroService.Entities;
using Microsoft.EntityFrameworkCore;

namespace DogMicroService.Repositories;

public class DogDbRepository(DogContext context) : IDogDbRepository
{
    // Créer un nouveau chien
    public async Task<Guid> AddDogAsync(DogEntity dog)
    {
        await context.Dogs.AddAsync(dog);
        await context.SaveChangesAsync();

        return dog.Id;
    }

    // Obtenir un chien par son ID
    public async Task<DogEntity?> GetDogByIdAsync(Guid id)
    {
        return await context.Dogs.FirstOrDefaultAsync(d => d.Id == id);
    }

    // Obtenir tous les chiens
    public async Task<List<DogEntity>> GetAllDogsAsync()
    {
        return await context.Dogs.ToListAsync();
    }

    // Mettre à jour un chien existant
    public async Task UpdateDogAsync(DogEntity dog)
    {
        context.Dogs.Update(dog);
        await context.SaveChangesAsync();
    }

    // Supprimer un chien par son ID
    public async Task DeleteDogAsync(Guid id)
    {
        DogEntity? dogToDelete = await context.Dogs.FirstOrDefaultAsync(d => d.Id == id);
        if (dogToDelete != null)
        {
            context.Dogs.Remove(dogToDelete);
            await context.SaveChangesAsync();
        }
    }

    
}