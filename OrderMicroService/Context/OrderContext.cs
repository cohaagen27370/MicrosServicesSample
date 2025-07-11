using Microsoft.EntityFrameworkCore;
using OrderMicroService.Entities;

namespace OrderMicroService.Context;

public class OrderContext(DbContextOptions<OrderContext> options) : DbContext(options)
{
    public DbSet<OrderEntity> Orders { get; set; } = null!;
    public DbSet<DogEntity> Dogs { get; set; } = null!;
}