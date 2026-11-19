namespace RoomHub.Api.Domain.Bookings;

public interface IBookingRepository
{
    Task<Booking?> GetByIdAsync(Guid id, CancellationToken ct);
    Task AddAsync(Booking booking, CancellationToken ct);

    // Only active bookings of the room: cancelled ones free their interval (R7, R8).
    Task<IReadOnlyList<Booking>> GetActiveForRoomAsync(Guid roomId, CancellationToken ct);

    // R9: active bookings of a participant that start on the given UTC day, in all rooms.
    Task<int> CountActiveForPersonOnDayAsync(string bookedBy, DateTime dayUtc, CancellationToken ct);
}
