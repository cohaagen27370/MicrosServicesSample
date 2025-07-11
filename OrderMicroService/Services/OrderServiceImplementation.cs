using DogService;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.EntityFrameworkCore;
using OrderMicroService.Context;
using OrderMicroService.Entities;
using OrderMicroService.Extensions;
using OrderService;

namespace OrderMicroService.Services;

public class OrderServiceImplementation(ILogger<OrderServiceImplementation> logger, OrderContext dbContext) : OrderGrpcService.OrderGrpcServiceBase
{
    private readonly ILogger<OrderServiceImplementation> _logger = logger;


    public override async Task<OrderResponse> AddDogInOrder(AddDogInOrderRequest request, ServerCallContext context)
    {
        try
        {
            OrderEntity? orderToUpdate = dbContext.Orders.SingleOrDefault(x => x.Id == Guid.Parse(request.OrderId));
            if (orderToUpdate == null)
                return new OrderResponse() { Success = false, Error = "order not found"};
    
            using GrpcChannel channel = GrpcChannel.ForAddress("http://localhost:5075");
            var client = new DogService.DogGrpcService.DogGrpcServiceClient(channel);
    
            OneDogResponse? dogResponse = client.GetOneDog(new GetDogRequest() { Id = request.DogServiceId });
    
            var newDog = dogResponse.Dog.ToDogEntity();
            dbContext.Dogs.Add(newDog);
            await dbContext.SaveChangesAsync();            
            orderToUpdate.Dogs.Add(newDog);
    
            await dbContext.SaveChangesAsync();
            
            return new OrderResponse() { Success = true, Id = orderToUpdate.Id.ToString()};
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during adding dog in order");
            return new OrderResponse() { Success = false, Error = ex.Message};  
        }   
    }
    
    public override async Task<OrderResponse> AddOrder(AddOrderRequest request, ServerCallContext context)
    {
        try
        {
            var newOrder = request.Order.ToOrderEntity();
            await dbContext.Orders.AddAsync(newOrder);
            await dbContext.SaveChangesAsync();
            return new OrderResponse() { Success = true, Id = newOrder.Id.ToString() };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during adding order");            
            return new OrderResponse() { Success = false, Error = ex.Message};  
        }
    }
    
    public override Task<ListOrdersResponse> ListOrders(ListOrdersRequest request, ServerCallContext context)
    {
        try
        {
            var result = new ListOrdersResponse();
            result.Orders.AddRange(dbContext.Orders.Include(x => x.Dogs).Select(x => x.ToOrder()).ToList());
            
            return Task.FromResult(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during listing orders");            
            return Task.FromResult(new ListOrdersResponse());  
        }     
    }
    
}