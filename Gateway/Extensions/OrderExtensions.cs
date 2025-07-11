using CommonDto.Models;
using Google.Protobuf.WellKnownTypes;
using OrderService;

namespace Gateway.Extensions;

public static class OrderExtensions
{


    public static DogDto ToDogDto(this OrderedDog dog)
    {
        return new DogDto()
        {
            Id = Guid.Parse(dog.Id),
            Name = dog.Name
        };
    }
    
    public static OrderedDog ToOrderedDogDto(this DogDto dog)
    {
        return new OrderedDog()
        {
            Id = dog.Id.ToString(),
            Name = dog.Name
        };
    }
    
    
    public static OrderDto ToOrderDto(this Order order)
    {
        return new OrderDto()
        {
            Id = Guid.Parse(order.Id),
            OrderDate = order.OrderDate.ToDateTime(),
            Dogs = order.Dogs.Select(x => x.ToDogDto()).ToList()
        };
    }
    
    public static Order ToOrder(this OrderDto order)
    {
        var result = new Order()
        {
            Id = order.Id.ToString(),
            OrderDate = order.OrderDate.ToTimestamp(),
        };
        
        result.Dogs.AddRange(order.Dogs.Select(x => x.ToOrderedDogDto()).ToList());

        return result;
    }    
    
}