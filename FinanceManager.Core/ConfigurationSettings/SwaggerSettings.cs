namespace FinanceManager.Core.ConfigurationSettings;

public record SwaggerSettings()
{
    public string Version { get; init; } = "";
    public string Title { get; init; } = "";
    
    public string Scheme { get; init; } = "";
    
    public string Description { get; init; } = "";
    
    public string Name { get; init; } = "";
    
    public string BearerFormat { get; init; } = "";
    
}