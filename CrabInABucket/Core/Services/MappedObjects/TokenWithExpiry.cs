namespace CrabInABucket.Core.Services.Dtos;

public record TokenWithExpiry(string Token, long ExpiryDate);