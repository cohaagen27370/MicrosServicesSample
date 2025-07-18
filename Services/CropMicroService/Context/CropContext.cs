using CropMicroService.Entities;
using Microsoft.EntityFrameworkCore;

namespace CropMicroService.Context;

public class CropContext(DbContextOptions<CropContext> options) : DbContext(options)
{
    public DbSet<CropEntity> Crops { get; set; } = null!;
}