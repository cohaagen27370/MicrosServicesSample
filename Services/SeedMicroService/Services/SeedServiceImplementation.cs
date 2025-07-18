using Grpc.Core;
using SeedMicroService.Extensions;
using SeedMicroService.Repositories;
using SeedService;

namespace SeedMicroService.Services;

public class SeedServiceImplementation(ILogger<SeedServiceImplementation> logger, ISeedDbRepository seedDbRepository) : 
    SeedGrpcService.SeedGrpcServiceBase
{
    public override async Task<SeedResponse> AddSeed(AddSeedRequest request, ServerCallContext context1)
    {
        try 
        {
            Guid id = await seedDbRepository.AddSeedAsync(request.Seed.ToEntity());
            
            return new SeedResponse() { Success = true, Id = id.ToString() };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during adding seed");
            return new SeedResponse() { Success = false, Error = ex.Message };
        }
    }

    public override async Task<SeedResponse> DeleteSeed(DeleteSeedRequest request, ServerCallContext context1)
    {
        try
        {
            await seedDbRepository.DeleteSeedAsync(Guid.Parse(request.Id));
            return new SeedResponse() { Success = true };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during deleting dog");            
            return new SeedResponse() { Success = false, Error = ex.Message };
        }
    }

    public override async Task<SeedResponse> UpdateSeed(UpdateSeedRequest request, ServerCallContext context1)
    {
        try
        {
            request.Seed.Id = request.Id;
            await seedDbRepository.UpdateSeedAsync(request.Seed.ToEntity());
            return new SeedResponse() { Success = true, Id = request.Id };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during updating dog");            
            return new SeedResponse() { Success = false, Error = ex.Message };
        }
    }

    public override async Task<ListSeedsResponse> ListSeeds(ListSeedsRequest request, ServerCallContext context1)
    {
        try
        {
            var result = new ListSeedsResponse();
            result.Seeds.AddRange((await seedDbRepository.GetAllSeedsAsync()).Select(x => x.ToSeed()));
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during listing dogs");            
            return new ListSeedsResponse();
        }
    }

    public override async Task<OneSeedResponse> GetOneSeed(GetSeedRequest request, ServerCallContext context1)
    {
        try
        {
            return new OneSeedResponse() { Seed = (await seedDbRepository.GetSeedByIdAsync(Guid.Parse(request.Id))).ToSeed()};
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during getting one dog");
            return new OneSeedResponse();
        }
    }

    public override async Task<ListCategoriesResponse> ListCategories(ListCategoriesRequest request, ServerCallContext context)
    {
        try
        {
            var result = new ListCategoriesResponse();
            result.Categories.AddRange((await seedDbRepository.GetAllCategoriesAsync()).Select(x => x.ToCategory()));
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during listing categories");            
            return new ListCategoriesResponse();
        }
    }
    
    public override async Task<GetCategoryResponse> GetOneCategory(GetCategoryRequest request, ServerCallContext context1)
    {
        try
        {
            return new GetCategoryResponse() { Category = (await seedDbRepository.GetCategoryByIdAsync(request.Id)).ToCategory()};
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during getting one dog");
            return new GetCategoryResponse();
        }
    }
    
}