using RoomHub.Api.Common;
using RoomHub.Api.Domain.Common;
using RoomHub.Api.Infrastructure;

namespace RoomHub.Api.Features.Bookings.CancelBooking;

public sealed record CancelBookingCommand(Guid Id) : ICommand<Guid>;

public sealed class CancelBookingHandler(BookingRepository bookings)
    : ICommandHandler<CancelBookingCommand, Guid>
{
    public Task<Result<Guid>> HandleAsync(CancelBookingCommand command, CancellationToken ct)
    {
        var booking = bookings.Get(command.Id);
        if (booking is null)
            return Task.FromResult<Result<Guid>>(
                Error.NotFound("Booking.NotFound", "The booking does not exist."));

        var cancelled = booking.Cancel();
        if (cancelled.IsFailure)
            return Task.FromResult(Result<Guid>.Failure(cancelled.Error));

        return Task.FromResult<Result<Guid>>(booking.Id);
    }
}
