namespace RoomHub.Api.Domain.Common;

// Saves all changes made through the repositories in one go.
public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken ct);
}
