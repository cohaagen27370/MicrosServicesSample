using DogService;
using Gateway.Extensions;
using Gateway.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers;

[ApiController]
[Route("[controller]/Dogs")]
public class ReferentielController(IDogRepository dogRepository) : ControllerBase
{
    [HttpGet("{dogId:guid}")]
    public IActionResult GetOneDog([FromRoute] Guid dogId)
    {
        try
        {
            return Ok(dogRepository.GetOneDogAsync(dogId).ToDogDto());
        }
        catch (Grpc.Core.RpcException ex)
        {
            return StatusCode(503, $"Erreur lors de l'appel gRPC: {ex.StatusCode} - {ex.Status.Detail}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erreur inattendue: {ex.Message}");
        }
    }
    
    [HttpGet("")]
    public async Task<IActionResult> GetAllDogs()
    {
        try
        {
            return Ok((await dogRepository.GetAllDogsAsync()).Select(x => x.ToDogDto()));
        }
        catch (Grpc.Core.RpcException ex)
        {
            return StatusCode(503, $"Erreur lors de l'appel gRPC: {ex.StatusCode} - {ex.Status.Detail}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erreur inattendue: {ex.Message}");
        }
    }
    
    [HttpPost("")]
    public async Task<IActionResult> AddDog([FromBody] Dog dog)
    {
        try
        {
            return Ok(await dogRepository.AddDog(dog));
        }
        catch (Grpc.Core.RpcException ex)
        {
            return StatusCode(500, $"Erreur inattendue: {ex.Message}"); 
        }
    }

    [HttpPut("{dogId:guid}")]
    public async Task<IActionResult> UpdateDog([FromRoute] Guid dogId, [FromBody] Dog dog)
    {
        try
        {
            return Ok(await dogRepository.UpdateDog(dogId, dog));
        }
        catch (Grpc.Core.RpcException ex)
        {
            return StatusCode(500, $"Erreur inattendue: {ex.Message}"); 
        }
    }
    
    [HttpDelete("{dogId:guid}")]
    public async Task<IActionResult> DeleteDog(Guid dogId)
    {
        try
        {
            await dogRepository.DeleteDog(dogId);
            return NoContent();
        }
        catch (Grpc.Core.RpcException ex)
        {
            return StatusCode(500, $"Erreur inattendue: {ex.Message}"); 
        }        
    }
    
}