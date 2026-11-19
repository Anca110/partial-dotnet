using RoomHub.Api.Common;
using RoomHub.Api.Domain.Common;
using RoomHub.Api.Infrastructure;

namespace RoomHub.Api.Features.Rooms.GetRoom;

public sealed record GetRoomQuery(Guid Id) : IQuery<GetRoomResponse>;

public sealed record GetRoomResponse(Guid Id, string Name, int Capacity);

public sealed class GetRoomHandler(RoomRepository rooms)
    : IQueryHandler<GetRoomQuery, GetRoomResponse>
{
    public Task<Result<GetRoomResponse>> HandleAsync(GetRoomQuery query, CancellationToken ct)
    {
        var room = rooms.Get(query.Id);
        if (room is null)
            return Task.FromResult<Result<GetRoomResponse>>(
                Error.NotFound("Room.NotFound", "The room does not exist."));

        return Task.FromResult<Result<GetRoomResponse>>(
            new GetRoomResponse(room.Id, room.Name, room.Capacity));
    }
}
