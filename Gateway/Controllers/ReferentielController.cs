using DogService;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers;

[ApiController]
[Route("Referentiel")]
public class DogController(DogService.DogService.DogServiceClient grpcClient) : ControllerBase
{
    private readonly DogService.DogService.DogServiceClient _grpcClient = grpcClient;

    [HttpGet("Dogs")]
    public async Task<IActionResult> GetAllDogs()
    {
        try
        {
            var request = new ListDogsRequest();
            var response = await _grpcClient.ListDogsAsync(request);
            return Ok(response.Dogs);
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
    
    
}