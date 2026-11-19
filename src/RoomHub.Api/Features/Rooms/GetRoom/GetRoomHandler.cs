using RoomHub.Api.Common;
using RoomHub.Api.Domain.Common;
using RoomHub.Api.Domain.Rooms;

namespace RoomHub.Api.Features.Rooms.GetRoom;

public sealed record GetRoomQuery(Guid Id) : IQuery<GetRoomResponse>;

public sealed record GetRoomResponse(Guid Id, string Name, int Capacity);

public sealed class GetRoomHandler(IRoomRepository rooms)
    : IQueryHandler<GetRoomQuery, GetRoomResponse>
{
    public async Task<Result<GetRoomResponse>> HandleAsync(GetRoomQuery query, CancellationToken ct)
    {
        var room = await rooms.GetByIdAsync(query.Id, ct);
        if (room is null)
            return Error.NotFound("Room.NotFound", "The room does not exist.");

        return new GetRoomResponse(room.Id, room.Name, room.Capacity);
    }
}
