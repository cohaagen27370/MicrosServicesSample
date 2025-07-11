using Microsoft.EntityFrameworkCore;
using OrderService.Entities;

namespace OrderService.Context;

public class OrderContext(DbContextOptions<OrderContext> options) : DbContext(options)
{
    public DbSet<OrderEntity> Orders { get; set; } = null!;
    public DbSet<DogEntity> Dogs { get; set; } = null!;
}