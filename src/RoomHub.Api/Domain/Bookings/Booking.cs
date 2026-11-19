using RoomHub.Api.Domain.Common;
using RoomHub.Api.Domain.Rooms;

namespace RoomHub.Api.Domain.Bookings;

public enum BookingStatus
{
    Active,
    Cancelled
}

public sealed class Booking
{
    public static readonly TimeSpan MaxDuration = TimeSpan.FromHours(4);

    public Guid Id { get; private set; }
    public Guid RoomId { get; private set; }
    public string BookedBy { get; private set; } = string.Empty;
    public DateTime StartUtc { get; private set; }
    public DateTime EndUtc { get; private set; }
    public int Attendees { get; private set; }
    public BookingStatus Status { get; private set; }

    private Booking() { }

    // R2-R7. "now" comes from TimeProvider (passed in by the handler), never from DateTime.UtcNow.
    public static Result<Booking> Create(
        Room room,
        string? bookedBy,
        DateTime startUtc,
        DateTime endUtc,
        int attendees,
        DateTime nowUtc,
        IEnumerable<Booking> roomBookings)
    {
        startUtc = ToUtc(startUtc);
        endUtc = ToUtc(endUtc);

        if (string.IsNullOrWhiteSpace(bookedBy))
            return Error.Validation("Booking.InvalidBookedBy", "BookedBy is required.");

        if (endUtc <= startUtc)
            return Error.Validation("Booking.InvalidRange", "The end must be after the start.");

        if (attendees <= 0)
            return Error.Validation("Booking.InvalidAttendees", "Attendees must be strictly positive.");

        if (attendees > room.Capacity)
            return Error.Validation("Booking.AttendeesExceedCapacity", "The room does not have enough seats.");

        if (startUtc < nowUtc)
            return Error.Validation("Booking.StartInPast", "A booking cannot start in the past.");

        if (endUtc - startUtc > MaxDuration)
            return Error.Validation("Booking.TooLong", "A booking can last at most 4 hours.");

        if (roomBookings.Any(b => b.Overlaps(startUtc, endUtc)))
            return Error.Conflict("Booking.RoomOccupied", "The room is occupied in this interval.");

        return new Booking
        {
            Id = Guid.NewGuid(),
            RoomId = room.Id,
            BookedBy = bookedBy.Trim(),
            StartUtc = startUtc,
            EndUtc = endUtc,
            Attendees = attendees,
            Status = BookingStatus.Active
        };
    }

    // Overlap check moved from the starter's BookingService as it was.
    public bool Overlaps(DateTime startUtc, DateTime endUtc) =>
        StartUtc <= endUtc && startUtc <= EndUtc;

    public Result<Booking> Cancel()
    {
        Status = BookingStatus.Cancelled;
        return this;
    }

    private static DateTime ToUtc(DateTime value) => value.Kind switch
    {
        DateTimeKind.Utc => value,
        DateTimeKind.Local => value.ToUniversalTime(),
        _ => DateTime.SpecifyKind(value, DateTimeKind.Utc)
    };
}
