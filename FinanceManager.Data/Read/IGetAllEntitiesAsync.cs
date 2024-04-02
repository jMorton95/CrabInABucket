using System.Collections;
using FinanceManager.Common.Entities;

namespace FinanceManager.Data.Read;

public interface IGetAllEntitiesAsync<TEntity> where TEntity : Entity
{
    Task<List<TEntity>> GetAllAsync();
}

public interface IGetAllOwnedEntitiesAsync<TEntity> where TEntity : Entity
{
    Task<List<TEntity>> GetAllOwnedEntitiesAsync(Guid userId);
}