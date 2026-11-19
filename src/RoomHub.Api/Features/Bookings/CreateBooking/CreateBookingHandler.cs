using RoomHub.Api.Common;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Domain.Common;
using RoomHub.Api.Infrastructure;

namespace RoomHub.Api.Features.Bookings.CreateBooking;

public sealed record CreateBookingCommand(
    Guid RoomId,
    string? BookedBy,
    DateTime StartUtc,
    DateTime EndUtc,
    int Attendees) : ICommand<CreateBookingResponse>;

public sealed record CreateBookingResponse(Guid Id, Guid RoomId, string Status);

public sealed class CreateBookingHandler(
    RoomRepository rooms,
    BookingRepository bookings,
    TimeProvider timeProvider)
    : ICommandHandler<CreateBookingCommand, CreateBookingResponse>
{
    public Task<Result<CreateBookingResponse>> HandleAsync(CreateBookingCommand command, CancellationToken ct)
    {
        // 404 before any validation (contract).
        var room = rooms.Get(command.RoomId);
        if (room is null)
            return Task.FromResult<Result<CreateBookingResponse>>(
                Error.NotFound("Room.NotFound", "The room does not exist."));

        var created = Booking.Create(
            room,
            command.BookedBy,
            command.StartUtc,
            command.EndUtc,
            command.Attendees,
            timeProvider.GetUtcNow().UtcDateTime,
            bookings.ListForRoom(room.Id));

        if (created.IsFailure)
            return Task.FromResult(Result<CreateBookingResponse>.Failure(created.Error));

        var booking = created.Value;
        bookings.Add(booking);

        return Task.FromResult<Result<CreateBookingResponse>>(
            new CreateBookingResponse(booking.Id, booking.RoomId, booking.Status.ToString()));
    }
}
