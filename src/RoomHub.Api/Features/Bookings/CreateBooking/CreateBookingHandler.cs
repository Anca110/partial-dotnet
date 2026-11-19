using RoomHub.Api.Common;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Domain.Common;
using RoomHub.Api.Domain.Rooms;

namespace RoomHub.Api.Features.Bookings.CreateBooking;

public sealed record CreateBookingCommand(
    Guid RoomId,
    string? BookedBy,
    DateTime StartUtc,
    DateTime EndUtc,
    int Attendees) : ICommand<CreateBookingResponse>;

public sealed record CreateBookingResponse(Guid Id, Guid RoomId, string Status);

public sealed class CreateBookingHandler(
    IRoomRepository rooms,
    IBookingRepository bookings,
    IUnitOfWork unitOfWork,
    TimeProvider timeProvider)
    : ICommandHandler<CreateBookingCommand, CreateBookingResponse>
{
    public async Task<Result<CreateBookingResponse>> HandleAsync(CreateBookingCommand command, CancellationToken ct)
    {
        // 404 before any validation (contract).
        var room = await rooms.GetByIdAsync(command.RoomId, ct);
        if (room is null)
            return Error.NotFound("Room.NotFound", "The room does not exist.");

        var roomBookings = await bookings.GetActiveForRoomAsync(room.Id, ct);

        var created = Booking.Create(
            room,
            command.BookedBy,
            command.StartUtc,
            command.EndUtc,
            command.Attendees,
            timeProvider.GetUtcNow().UtcDateTime,
            roomBookings);

        if (created.IsFailure)
            return created.Error;

        var booking = created.Value;
        await bookings.AddAsync(booking, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return new CreateBookingResponse(booking.Id, booking.RoomId, booking.Status.ToString());
    }
}
