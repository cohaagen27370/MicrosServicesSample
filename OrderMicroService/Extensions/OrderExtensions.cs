using DogService;
using Google.Protobuf.WellKnownTypes;
using OrderMicroService.Entities;
using OrderService;

namespace OrderMicroService.Extensions;

public static class OrderExtensions
{
    
    public static DogEntity ToDogEntity(this OrderedDog dog)
    {
        return new DogEntity
        {
            Id = dog.Id != null ? Guid.Parse(dog.Id) : Guid.NewGuid(),
            Name = dog.Name
        };
    }
    
    public static DogEntity ToDogEntity(this Dog dog)
    {
        return new DogEntity
        {
            Id = dog.Id != null ? Guid.Parse(dog.Id) : Guid.NewGuid(),
            Name = dog.Name,
            IdFromDogService = Guid.Parse(dog.Id!)
        };
    }    

    public static OrderedDog ToDog(this DogEntity dogEntity)
    {
        return new OrderedDog()
        {
            Id = dogEntity.Id.ToString(),
            Name = dogEntity.Name,
            IdFromDogService = dogEntity.IdFromDogService.ToString()
        };
    }
    
    
    
    public static OrderEntity ToOrderEntity(this Order order)
    {
        var newEntity = new OrderEntity
        {
            Id = order.Id != null ? Guid.Parse(order.Id) : Guid.NewGuid(),
            OrderDate = order.OrderDate.ToDateTime(),
            Dogs = order.Dogs.Select(x => x.ToDogEntity()).ToList()
        };

        return newEntity;
    }

    public static Order ToOrder(this OrderEntity orderEntity)
    {
        var newOrder = new Order()
        {
            Id = orderEntity.Id.ToString(),
            OrderDate = orderEntity.OrderDate.ToTimestamp()
        };

        newOrder.Dogs.AddRange(orderEntity.Dogs.Select(x => x.ToDog()));

        return newOrder;
    }
}