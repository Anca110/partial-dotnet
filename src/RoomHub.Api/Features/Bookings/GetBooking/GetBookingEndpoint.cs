using RoomHub.Api.Common;

namespace RoomHub.Api.Features.Bookings.GetBooking;

public static class GetBookingEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/bookings/{id:guid}", async (
            Guid id,
            IQueryHandler<GetBookingQuery, GetBookingResponse> handler,
            CancellationToken ct) =>
        {
            var result = await handler.HandleAsync(new GetBookingQuery(id), ct);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : result.Error.ToProblem();
        }).WithTags("Bookings");
    }
}
