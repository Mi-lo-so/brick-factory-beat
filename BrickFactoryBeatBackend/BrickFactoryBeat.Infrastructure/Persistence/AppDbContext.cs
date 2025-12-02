using BrickFactoryBeat.Domain.Equipment;
using BrickFactoryBeat.Domain.Orders;
using BrickFactoryBeat.Domain.StateHistory;
using Microsoft.EntityFrameworkCore;

namespace BrickFactoryBeat.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Equipment> Equipment { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<StateHistoryRecord> StateHistory { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>()
            .HasOne(o => o.Equipment)
            .WithMany(e => e.Orders)
            .HasForeignKey(o => o.EquipmentId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Optional: ensure Name is TEXT for SQLite
        // (since SQLServer doesn't work on Mac, oops)
        modelBuilder.Entity<Equipment>()
            .Property(e => e.Name)
            .IsRequired();
        
        base.OnModelCreating(modelBuilder);
    }
}
