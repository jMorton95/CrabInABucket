using System.Collections;
using FinanceManager.Common.DataEntities;

namespace FinanceManager.Data.Read;

public interface IGetAllEntitiesAsync<TEntity> where TEntity : Entity
{
    Task<IEnumerable<TEntity>> GetAllAsync();
}

public interface IGetAllOwnedEntitiesAsync<TEntity> where TEntity : Entity
{
    Task<IEnumerable<TEntity>> GetAllOwnedEntitiesAsync(Guid userId);
}