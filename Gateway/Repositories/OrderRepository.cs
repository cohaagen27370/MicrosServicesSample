using Google.Protobuf.Collections;
using OrderService;

namespace Gateway.Repositories;

public class OrderRepository(OrderService.OrderGrpcService.OrderGrpcServiceClient grpcClient) : IOrderRepository
{
    public async Task<Guid> AddOrder(Order newOrder)
    {
        var request = new AddOrderRequest() { Order = newOrder };
        OrderResponse? response = await grpcClient.AddOrderAsync(request);
        return Guid.Parse(response.Id);
    }

    public async Task<Guid> AddDogInOrder(Guid orderId, Guid dogId)
    {
        var request = new AddDogInOrderRequest() { DogServiceId = dogId.ToString(), OrderId = orderId.ToString() };
        OrderResponse? response = await grpcClient.AddDogInOrderAsync(request);
        return Guid.Parse(response.Id);
    }

    public async Task<RepeatedField<Order>> GetAllOrders()
    {
        var request = new ListOrdersRequest();
        ListOrdersResponse? response = await grpcClient.ListOrdersAsync(request);
        return response.Orders;
    }
    
}