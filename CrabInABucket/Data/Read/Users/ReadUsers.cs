using CrabInABucket.Data.Read.Generic;
using CrabInABucket.Data.Models;

namespace CrabInABucket.Data.Read.Users;

public interface IReadUsers : IRead<User> { }

public sealed class ReadUsers : IReadUsers
{
    public Task<User?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<User>> GetAllAsync()
    {
        throw new NotImplementedException();
    }
}