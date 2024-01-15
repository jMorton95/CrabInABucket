using FinanceManager.Core.DataEntities;
using FinanceManager.Core.Requests;

namespace FinanceManager.Data.Write;

public interface IWrite<in TEntity, in TEditRequest> where TEntity : BaseModel where TEditRequest : BaseEditRequest
{
    Task<int> CreateAsync(TEntity entity);
    
    Task<int> EditAsync(TEditRequest request);
}