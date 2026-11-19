using RoomHub.Api.Common;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Domain.Common;

namespace RoomHub.Api.Features.Bookings.CancelBooking;

public sealed record CancelBookingCommand(Guid Id) : ICommand<Guid>;

public sealed class CancelBookingHandler(IBookingRepository bookings, IUnitOfWork unitOfWork)
    : ICommandHandler<CancelBookingCommand, Guid>
{
    public async Task<Result<Guid>> HandleAsync(CancelBookingCommand command, CancellationToken ct)
    {
        var booking = await bookings.GetByIdAsync(command.Id, ct);
        if (booking is null)
            return Error.NotFound("Booking.NotFound", "The booking does not exist.");

        var cancelled = booking.Cancel();
        if (cancelled.IsFailure)
            return cancelled.Error;

        await unitOfWork.SaveChangesAsync(ct);
        return booking.Id;
    }
}
