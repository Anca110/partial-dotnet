using Microsoft.EntityFrameworkCore;
using RoomHub.Api.Domain.Bookings;

namespace RoomHub.Api.Infrastructure.Persistence;

public sealed class EfBookingRepository(RoomHubDbContext db) : IBookingRepository
{
    public async Task<Booking?> GetByIdAsync(Guid id, CancellationToken ct) =>
        await db.Bookings.FindAsync([id], ct);

    public async Task AddAsync(Booking booking, CancellationToken ct) =>
        await db.Bookings.AddAsync(booking, ct);

    public async Task<IReadOnlyList<Booking>> GetActiveForRoomAsync(Guid roomId, CancellationToken ct) =>
        await db.Bookings
            .Where(b => b.RoomId == roomId && b.Status == BookingStatus.Active)
            .ToListAsync(ct);
}
