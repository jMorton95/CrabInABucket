using CrabInABucket.Core.Services.Dtos;

namespace CrabInABucket.Api.Responses;

public record LoginResponse(TokenDto AccessToken, UserResponse User);