using CrabInABucket.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CrabInABucket.Data.Access.Generic;

public class Read<TEntity> : IRead<TEntity> where TEntity : BaseModel
{
    private readonly DataContext _db;
    
    public Read(DataContext db)
    {
        _db = db;
    }
    
    public virtual Task<TEntity?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public virtual Task<ICollection<TEntity>> GetAllAsync()
    {
        throw new NotImplementedException();
    }
}