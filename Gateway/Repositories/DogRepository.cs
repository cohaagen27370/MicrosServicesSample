using DogService;
using Gateway.Extensions;
using Google.Protobuf.Collections;

namespace Gateway.Repositories;

public class DogRepository(DogService.DogGrpcService.DogGrpcServiceClient grpcClient) : IDogRepository
{
    
    public Dog GetOneDogAsync(Guid id)
    {
        var request = new GetDogRequest() { Id = id.ToString()};
        OneDogResponse? response = grpcClient.GetOneDog(request);
        return response.Dog;
    }

    public async Task<RepeatedField<Dog>> GetAllDogsAsync()
    {
        var request = new ListDogsRequest();
        ListDogsResponse? response = await grpcClient.ListDogsAsync(request);
        return response.Dogs;
    }

    public async Task<Guid> AddDog(Dog newDog)
    {
        var request = new AddDogRequest() { Dog = newDog };
        DogResponse? response = await grpcClient.AddDogAsync(request);
        return Guid.Parse(response.Id);
    }

    public async Task<Guid> UpdateDog(Guid dogId, Dog updatedDog)
    {
        var request = new UpdateDogRequest { Id = dogId.ToString(), Dog = updatedDog };
        DogResponse? response = await grpcClient.UpdateDogAsync(request);
        return Guid.Parse(response.Id);
    }

    public async Task DeleteDog(Guid dogId)
    {
        var request = new DeleteDogRequest { Id = dogId.ToString() };
        DogResponse? response = await grpcClient.DeleteDogAsync(request);
    }
    
    
    
    
    
}