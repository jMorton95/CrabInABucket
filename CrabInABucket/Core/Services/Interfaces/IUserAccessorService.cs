namespace CrabInABucket.Core.Services.Interfaces;

public interface IUserAccessorService
{
    Guid? GetCurrentUserId();
}