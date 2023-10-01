using CrabInABucket.Data.Models;

namespace CrabInABucket.Data.Access.Generic;

public interface IRead<TEntity> where TEntity : BaseModel
{
    Task<TEntity?> GetByIdAsync(Guid id);

    Task<ICollection<TEntity>> GetAllAsync();
}