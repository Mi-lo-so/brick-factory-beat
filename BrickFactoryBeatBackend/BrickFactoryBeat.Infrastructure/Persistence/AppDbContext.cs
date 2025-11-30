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
            .OnDelete(DeleteBehavior.Restrict); //  trying to avoid cascading loops

        base.OnModelCreating(modelBuilder);
    }
}