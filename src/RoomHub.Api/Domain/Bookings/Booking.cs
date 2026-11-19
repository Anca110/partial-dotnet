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
    public static readonly TimeSpan MaxDurationLargeRoom = TimeSpan.FromHours(2);
    public const int LargeRoomCapacity = 10;
    public const int MaxActiveBookingsPerDay = 2;
    public static readonly TimeSpan MinCancelNotice = TimeSpan.FromMinutes(60);

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
        IEnumerable<Booking> roomBookings,
        int activeBookingsSameDayForPerson)
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

        // R10: rooms with capacity greater than 10 can be booked for at most 2 hours.
        if (room.Capacity > LargeRoomCapacity && endUtc - startUtc > MaxDurationLargeRoom)
            return Error.Validation("Booking.TooLongForLargeRoom", "Rooms with more than 10 seats can be booked for at most 2 hours.");

        if (roomBookings.Any(b => b.Overlaps(startUtc, endUtc)))
            return Error.Conflict("Booking.RoomOccupied", "The room is occupied in this interval.");

        // R9: at most 2 active bookings per participant per UTC day, across all rooms.
        if (activeBookingsSameDayForPerson >= MaxActiveBookingsPerDay)
            return Error.Conflict("Booking.DailyLimitReached", "A participant can have at most 2 active bookings per day.");

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

    // R7: only active bookings block the room. Strict "<": intervals that only touch
    // (one ends exactly when the other starts) do not overlap.
    public bool Overlaps(DateTime startUtc, DateTime endUtc) =>
        Status == BookingStatus.Active && StartUtc < endUtc && startUtc < EndUtc;

    // R8: Active -> Cancelled. Cancelling twice is a conflict.
    // R11: only if the booking starts at least 60 minutes from now.
    public Result<Booking> Cancel(DateTime nowUtc)
    {
        if (Status == BookingStatus.Cancelled)
            return Error.Conflict("Booking.AlreadyCancelled", "The booking is already cancelled.");

        if (StartUtc - nowUtc < MinCancelNotice)
            return Error.Conflict("Booking.TooLateToCancel", "A booking can be cancelled only at least 60 minutes before it starts.");

        Status = BookingStatus.Cancelled;
        return this;
    }

    public static DateTime ToUtc(DateTime value) => value.Kind switch
    {
        DateTimeKind.Utc => value,
        DateTimeKind.Local => value.ToUniversalTime(),
        _ => DateTime.SpecifyKind(value, DateTimeKind.Utc)
    };
}
