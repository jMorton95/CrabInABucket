namespace CrabInABucket.Core.Configurators;

public class JwtSettings
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string Key { get; set; }
    public int ExpireDays { get; set; }
}