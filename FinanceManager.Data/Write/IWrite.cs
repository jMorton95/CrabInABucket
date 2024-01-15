using FinanceManager.Core.DataEntities;
using FinanceManager.Core.Requests;

namespace FinanceManager.Data.Write;

public interface IWrite<in TEntity, in TEditRequest> where TEntity : BaseModel where TEditRequest : BaseEditRequest
{
    Task<bool> CreateAsync(TEntity entity);
    
    Task<bool> EditAsync(TEditRequest request);
}