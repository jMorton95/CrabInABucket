using CrabInABucket.Core.Services.Dtos;

namespace CrabInABucket.Api.Responses;

public record LoginResponse(TokenWithExpiry AccessToken, UserResponse User);