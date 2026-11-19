namespace RoomHub.Api.Domain.Bookings;

public interface IBookingRepository
{
    Task<Booking?> GetByIdAsync(Guid id, CancellationToken ct);
    Task AddAsync(Booking booking, CancellationToken ct);

    // Only active bookings of the room: cancelled ones free their interval (R7, R8).
    Task<IReadOnlyList<Booking>> GetActiveForRoomAsync(Guid roomId, CancellationToken ct);
}
