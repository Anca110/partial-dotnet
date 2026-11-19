using RoomHub.Api.Domain.Rooms;

namespace RoomHub.Api.Infrastructure.Persistence;

public sealed class EfRoomRepository(RoomHubDbContext db) : IRoomRepository
{
    public async Task<Room?> GetByIdAsync(Guid id, CancellationToken ct) =>
        await db.Rooms.FindAsync([id], ct);

    public async Task AddAsync(Room room, CancellationToken ct) =>
        await db.Rooms.AddAsync(room, ct);
}
