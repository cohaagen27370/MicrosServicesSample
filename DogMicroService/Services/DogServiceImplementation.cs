using DogMicroService.Repositories;
using Grpc.Core;
using DogMicroService.Extensions;
using DogService;

namespace DogMicroService.Services;

public class DogServiceImplementation(ILogger<DogServiceImplementation> logger, IDogDbRepository dogDbRepository) : 
    DogGrpcService.DogGrpcServiceBase
{
    public override async Task<DogResponse> AddDog(AddDogRequest request, ServerCallContext context1)
    {
        try 
        {
            Guid id = await dogDbRepository.AddDogAsync(request.Dog.ToEntity());
            
            return new DogResponse() { Success = true, Id = id.ToString() };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during adding dog");
            return new DogResponse() { Success = false, Error = ex.Message };
        }
    }

    public override async Task<DogResponse> DeleteDog(DeleteDogRequest request, ServerCallContext context1)
    {
        try
        {
            await dogDbRepository.DeleteDogAsync(Guid.Parse(request.Id));
            return new DogResponse() { Success = true };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during deleting dog");            
            return new DogResponse() { Success = false, Error = ex.Message };
        }
    }

    public override async Task<DogResponse> UpdateDog(UpdateDogRequest request, ServerCallContext context1)
    {
        try
        {
            request.Dog.Id = request.Id;
            await dogDbRepository.UpdateDogAsync(request.Dog.ToEntity());
            return new DogResponse() { Success = true, Id = request.Id };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during updating dog");            
            return new DogResponse() { Success = false, Error = ex.Message };
        }
    }

    public override async Task<ListDogsResponse> ListDogs(ListDogsRequest request, ServerCallContext context1)
    {
        try
        {
            var result = new ListDogsResponse();
            result.Dogs.AddRange((await dogDbRepository.GetAllDogsAsync()).Select(x => x.ToDog()));
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during listing dogs");            
            return new ListDogsResponse();
        }
    }

    public override async Task<OneDogResponse> GetOneDog(GetDogRequest request, ServerCallContext context1)
    {
        try
        {
            return new OneDogResponse() { Dog = (await dogDbRepository.GetDogByIdAsync(Guid.Parse(request.Id))).ToDog()};
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during getting one dog");
            return new OneDogResponse();
        }
    }

}