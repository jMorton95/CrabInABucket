using System.Net;
using FinanceManager.Api.Features.Auth;
using FinanceManager.Common.Entities;

namespace FinanceManager.Tests.Integration;

public class AuthTests(IntegrationTestApplicationFactory factory, SharedContainerFixture _) : BaseIntegrationTest(factory)
{
    public static List<object[]> InvalidRegistrations =>
    [
        [new Register.Request("invalid-email", "ValidPass123!", "ValidPass123!"), "Username"],
        [new Register.Request("", "ValidPass123!", "ValidPass123!"), "Username"],
        [new Register.Request("valid@email.com", "", ""), "Password"],
        [new Register.Request("valid@email.com", "Short1", "Short1"), "Password"],
        [new Register.Request("valid@email.com", new string('a', 21), new string('a', 21)), "Password"],
        [new Register.Request("valid@email.com", "ValidPass123!", ""), "PasswordConfirmation"],
        [new Register.Request("valid@email.com", "ValidPass123!", "Short1"), "PasswordConfirmation"],
        [new Register.Request("valid@email.com", "ValidPass123!", new string('a', 21)), "PasswordConfirmation"],
        [new Register.Request("valid@email.com", "ValidPass123!", "DifferentPass123!"), "PasswordConfirmation"]
    ];

    public static List<object[]> ExistingUser => [[new Register.Request("existing@email.com", "ValidPass123!", "ValidPass123!"), "Username"]];
    
    [Theory, MemberData(nameof(InvalidRegistrations))]
    public async Task TestRegistrationValidation(Register.Request request, string validationParameterError)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/auth/register", request);
        
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        
        var problemResult = await response.Content.ReadFromJsonAsync<HttpValidationProblemDetails>();
        
        Assert.NotNull(problemResult?.Errors);
        Assert.Contains(problemResult.Errors, (error) => error.Key == validationParameterError);
    }

    [Theory, MemberData(nameof(ExistingUser))]
    public async Task TestExistingUserValidation(Register.Request request, string validationParameterError)
    {
        var existingUser = new User() { Username = request.Username, Password = request.Password };
        _dataContext.User.Add(existingUser);
        await _dataContext.SaveChangesAsync();
        
        var response = await _httpClient.PostAsJsonAsync("/api/auth/register", request);
        
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        
        var problemResult = await response.Content.ReadFromJsonAsync<HttpValidationProblemDetails>();
        
        Assert.NotNull(problemResult?.Errors);
        Assert.Contains(problemResult.Errors, (error) => error.Key == validationParameterError);
    }
}