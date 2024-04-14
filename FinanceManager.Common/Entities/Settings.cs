namespace FinanceManager.Common.Entities;

public class Settings : Entity
{
    public bool ShouldSimulate { get; set; } = false;
    public bool HasBeenSimulated { get; set; } = false;
    public bool ShouldOverwrite { get; set; } = false;
}