using CrabInABucket.Data.Models;

namespace CrabInABucket.Data.Read.Generic;

public interface IRead<TEntity> where TEntity : BaseModel
{
    Task<TEntity?> GetByIdAsync(Guid id);

    Task<IEnumerable<TEntity>> GetAllAsync();
}