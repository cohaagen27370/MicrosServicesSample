using CommonDto.Models;
using Gateway.Repositories;
using Microsoft.AspNetCore.Mvc;
using Gateway.Extensions;

namespace Gateway.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController(IOrderRepository orderRepository) : ControllerBase
{
    
    [HttpPost("")]
    public async Task<IActionResult> AddOrder([FromBody] OrderDto order)
    {
        try
        {
            return Ok(await orderRepository.AddOrder(order.ToOrder()));
        }
        catch (Grpc.Core.RpcException ex)
        {
            return StatusCode(500, $"Erreur inattendue: {ex.Message}"); 
        }
    }
    
    [HttpPost("{orderId:guid}/dogs/{dogId:guid}")]
    public async Task<IActionResult> AddDogInOrder([FromRoute] Guid orderId, [FromRoute] Guid dogId)
    {
        try
        {
            return Ok(await orderRepository.AddDogInOrder(orderId, dogId));            
        }
        catch (Grpc.Core.RpcException ex)
        {
            return StatusCode(500, $"Erreur inattendue: {ex.Message}"); 
        }
    }
    
    [HttpGet("")]
    public async Task<IActionResult> GetAllOrders()
    {
        try
        {
            return Ok((await orderRepository.GetAllOrders()).Select(x => x.ToOrderDto()));   
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