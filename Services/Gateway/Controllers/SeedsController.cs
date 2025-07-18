using CommonDto.Models;
using Gateway.Extensions;
using Gateway.Repositories;
using Google.Protobuf.Collections;
using Microsoft.AspNetCore.Mvc;
using SeedService;

namespace Gateway.Controllers;

[ApiController]
[Route("[controller]")]
public class SeedsController(ISeedRepository seedRepository) : ControllerBase
{
    [HttpGet("{seedId:guid}")]
    public async Task<IActionResult> GetOneSeed([FromRoute] Guid seedId)
    {
        Seed seed = await seedRepository.GetOneSeedAsync(seedId);
        
        var seedDto = seed.ToSeedDto();
        seedDto.Category = (await seedRepository.GetOneCategoryAsync(seed.CategoryId)).ToCategoryDto();
        
        return Ok(seedDto);
    }
    
    [HttpGet("")]
    public async Task<IActionResult> GetAllSeeds()
    {
        RepeatedField<Seed> seeds = await seedRepository.GetAllSeedsAsync();
        IEnumerable<SeedDto> results = seeds.Select(x => x.ToSeedDto());
        List<SeedDto> resultsList = [];
        foreach (SeedDto result in results)
        {
            result.Category = (await seedRepository.GetOneCategoryAsync(result.Category.Id)).ToCategoryDto();
            resultsList.Add(result);           
        }
        return Ok(resultsList);
    }
    
    [HttpPost("")]
    public async Task<IActionResult> AddSeed([FromBody] SeedDto seed)
    {
        return Ok(await seedRepository.AddSeed(seed.ToSeed()));
    }
    
    [HttpPut("{seedId:guid}")]
    public async Task<IActionResult> UpdateSeed([FromRoute] Guid seedId, [FromBody] SeedDto seed)
    {
        return Ok(await seedRepository.UpdateSeed(seedId, seed.ToSeed()));
    }
    
    [HttpDelete("{seedId:guid}")]
    public async Task<IActionResult> DeleteSeed(Guid seedId)
    {
        await seedRepository.DeleteSeed(seedId);
        return NoContent();
    }
    
}