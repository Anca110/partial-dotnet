using Microsoft.EntityFrameworkCore;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Domain.Rooms;

namespace RoomHub.Api.Infrastructure.Persistence;

public sealed class RoomHubDbContext(DbContextOptions<RoomHubDbContext> options) : DbContext(options)
{
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<Booking> Bookings => Set<Booking>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Room>(room =>
        {
            room.HasKey(r => r.Id);
            room.Property(r => r.Name).IsRequired().HasMaxLength(200);
        });

        modelBuilder.Entity<Booking>(booking =>
        {
            booking.HasKey(b => b.Id);
            booking.Property(b => b.BookedBy).IsRequired().HasMaxLength(200);
            booking.Property(b => b.Status).HasConversion<string>();

            // SQLite does not keep DateTime.Kind: mark the values read back as UTC,
            // so the API returns them with "Z".
            booking.Property(b => b.StartUtc).HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
            booking.Property(b => b.EndUtc).HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

            booking.HasOne<Room>().WithMany().HasForeignKey(b => b.RoomId);
            booking.HasIndex(b => b.RoomId);
        });
    }
}
