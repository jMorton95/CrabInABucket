using CrabInABucket.Data.Models;

namespace CrabInABucket.Data.Write.Generic;

public interface IWrite<in TEntity> where TEntity : BaseModel
{
    Task<int> CreateAsync(TEntity entity);
    
    Task<int> EditAsync(TEntity entity);
}