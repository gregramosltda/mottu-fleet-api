using Microsoft.EntityFrameworkCore;
using Mottu.Fleet.Api.Models;

namespace Mottu.Fleet.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Moto> Motos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Moto>()
            .HasIndex(m => m.Placa)
            .IsUnique();
    }
}
