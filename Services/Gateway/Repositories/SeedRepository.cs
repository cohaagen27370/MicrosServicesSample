using CommonDto.Models;
using Google.Protobuf.Collections;
using Grpc.Core;
using SeedService;

namespace Gateway.Repositories;

public class SeedRepository(SeedGrpcService.SeedGrpcServiceClient grpcClient, IConfiguration configuration) : ISeedRepository
{

    public async Task<Seed> GetOneSeedAsync(Guid id)
    {
        var request = new GetSeedRequest() { Id = id.ToString()};
        OneSeedResponse? response = await grpcClient.GetOneSeedAsync(request);
        return response.Seed;
    }

    public async Task<Category> GetOneCategoryAsync(int id)
    {
        var request = new GetCategoryRequest() { Id = id};
        GetCategoryResponse? response = await grpcClient.GetOneCategoryAsync(request);
        return response.Category;
    }

    public async Task<RepeatedField<Seed>> GetAllSeedsAsync()
    {
        var request = new ListSeedsRequest();
        ListSeedsResponse? response = await grpcClient.ListSeedsAsync(request);
        return response.Seeds;
    }

    public async Task<Guid> AddSeed(Seed newSeed)
    {
        var request = new AddSeedRequest() { Seed = newSeed };
        SeedResponse? response = await grpcClient.AddSeedAsync(request);
        return Guid.Parse(response.Id);
    }

    public async Task<Guid> UpdateSeed(Guid seedId, Seed updatedSeed)
    {
        var request = new UpdateSeedRequest { Id = seedId.ToString(), Seed = updatedSeed };
        SeedResponse? response = await grpcClient.UpdateSeedAsync(request);
        return Guid.Parse(response.Id);
    }

    public async Task DeleteSeed(Guid seedId)
    {
        var request = new DeleteSeedRequest { Id = seedId.ToString() };
        SeedResponse? response = await grpcClient.DeleteSeedAsync(request);
    }
    
    
    
    
    
}