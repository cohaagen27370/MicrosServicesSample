using Google.Protobuf.Collections;
using OrderService;

namespace Gateway.Repositories;

public interface IOrderRepository
{
    Task<Guid> AddOrder(Order newOrder);

    Task<Guid> AddDogInOrder(Guid orderId, Guid dogId);

    Task<RepeatedField<Order>> GetAllOrders();
}