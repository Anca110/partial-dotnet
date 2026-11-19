using RoomHub.Api.Common;
using RoomHub.Api.Domain.Common;
using RoomHub.Api.Domain.Rooms;
using RoomHub.Api.Infrastructure;

namespace RoomHub.Api.Features.Rooms.CreateRoom;

public sealed record CreateRoomCommand(string? Name, int Capacity) : ICommand<CreateRoomResponse>;

public sealed record CreateRoomResponse(Guid Id, string Name, int Capacity);

public sealed class CreateRoomHandler(RoomRepository rooms)
    : ICommandHandler<CreateRoomCommand, CreateRoomResponse>
{
    public Task<Result<CreateRoomResponse>> HandleAsync(CreateRoomCommand command, CancellationToken ct)
    {
        var created = Room.Create(command.Name, command.Capacity);
        if (created.IsFailure)
            return Task.FromResult(Result<CreateRoomResponse>.Failure(created.Error));

        var room = created.Value;
        rooms.Add(room);

        return Task.FromResult<Result<CreateRoomResponse>>(
            new CreateRoomResponse(room.Id, room.Name, room.Capacity));
    }
}
