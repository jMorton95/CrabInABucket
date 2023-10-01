using CrabInABucket.Data.Models;

namespace CrabInABucket.Data.Read.Generic;

public interface IRead<TEntity> where TEntity : BaseModel
{
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<TEntity?> GetByIdAsync(Guid id);

}