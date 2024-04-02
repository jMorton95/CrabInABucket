namespace FinanceManager.Data.Write;

public interface IEditEntity<in TEditRequest>
{
    Task<bool> EditAsync(TEditRequest request);
}