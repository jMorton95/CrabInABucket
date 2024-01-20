using FinanceManager.Core.Requests;

namespace FinanceManager.Data.Write;

public interface IEditEntity<in TEditRequest> where TEditRequest : BaseEditRequest
{
    Task<bool> EditAsync(TEditRequest request);
}