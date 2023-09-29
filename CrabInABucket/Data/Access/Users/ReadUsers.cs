using CrabInABucket.Data.Access.Generic;
using CrabInABucket.Data.Models;

namespace CrabInABucket.Data.Access;

public class ReadUsers<User> : IRead<User> where User : BaseModel
{
    public Task<User?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<ICollection<User>> GetAllAsync()
    {
        throw new NotImplementedException();
    }
}