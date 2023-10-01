namespace CrabInABucket.Application.ConfigurationSettings;

public record JwtSettings()
{
    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
    public string Key { get; init; } = string.Empty;
    public int ExpireDays { get; init; } = 1;

}
    
