using RoomHub.Api.Common;
using RoomHub.Api.Domain.Common;
using RoomHub.Api.Infrastructure;

namespace RoomHub.Api.Features.Bookings.GetBooking;

public sealed record GetBookingQuery(Guid Id) : IQuery<GetBookingResponse>;

public sealed record GetBookingResponse(
    Guid Id,
    Guid RoomId,
    string BookedBy,
    DateTime StartUtc,
    DateTime EndUtc,
    int Attendees,
    string Status);

public sealed class GetBookingHandler(BookingRepository bookings)
    : IQueryHandler<GetBookingQuery, GetBookingResponse>
{
    public Task<Result<GetBookingResponse>> HandleAsync(GetBookingQuery query, CancellationToken ct)
    {
        var b = bookings.Get(query.Id);
        if (b is null)
            return Task.FromResult<Result<GetBookingResponse>>(
                Error.NotFound("Booking.NotFound", "The booking does not exist."));

        return Task.FromResult<Result<GetBookingResponse>>(new GetBookingResponse(
            b.Id, b.RoomId, b.BookedBy, b.StartUtc, b.EndUtc, b.Attendees, b.Status.ToString()));
    }
}
