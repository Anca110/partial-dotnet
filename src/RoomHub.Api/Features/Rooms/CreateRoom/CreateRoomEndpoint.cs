using RoomHub.Api.Common;

namespace RoomHub.Api.Features.Rooms.CreateRoom;

public sealed record CreateRoomRequest(string? Name, int Capacity);

public static class CreateRoomEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/rooms", async (
            CreateRoomRequest request,
            ICommandHandler<CreateRoomCommand, CreateRoomResponse> handler,
            CancellationToken ct) =>
        {
            var result = await handler.HandleAsync(new CreateRoomCommand(request.Name, request.Capacity), ct);

            return result.IsSuccess
                ? Results.Created($"/api/rooms/{result.Value.Id}", result.Value)
                : result.Error.ToProblem();
        }).WithTags("Rooms");
    }
}
