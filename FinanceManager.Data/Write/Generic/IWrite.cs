using FinanceManager.Common.Models;
using FinanceManager.Common.Models;

namespace FinanceManager.Data.Write.Generic;

public interface IWrite<in TEntity> where TEntity : BaseModel
{
    Task<int> CreateAsync(User entity);
    
    Task<int> EditAsync(TEntity entity);
}