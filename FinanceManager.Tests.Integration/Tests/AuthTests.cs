using System.Net;
using FinanceManager.Api.Endpoints.Auth;
using FinanceManager.Common.Constants;
using FinanceManager.Tests.Integration.Setup;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Tests.Integration.Tests;

[Collection("Integration Test Collection")]
public class AuthTests(IntegrationTestContext context)
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

    public static List<object[]> InvalidLogins =>
    [
        [new Login.Request("invalid-email", "ValidPass123!"), "Username"],
        [new Login.Request("", "ValidPass123!"), "Username"],
        [new Login.Request("valid@email.com", ""), "Password"],
        [new Login.Request("valid@email.com", "Short1"), "Password"],
        [new Login.Request("valid@email.com", new string('a', 21)), "Password"]
    ];

    public static List<object[]> ExistingUser => [[new Register.Request(TestConstants.Username, TestConstants.Password, TestConstants.Password), "Username"]];
    
    [Theory, MemberData(nameof(InvalidRegistrations))]
    public async Task TestRegistrationValidation(Register.Request request, string erroredValidationProperty)
    {
        var response = await context.HttpClient.PostAsJsonAsync("/api/auth/register", request);
        
        var problemResult = await response.Content.ReadFromJsonAsync<HttpValidationProblemDetails>();
        
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.NotNull(problemResult?.Errors);
        Assert.Contains(problemResult.Errors, (error) => error.Key == erroredValidationProperty);
    }

    [Theory, MemberData(nameof(ExistingUser))]
    public async Task TestExistingUserValidation(Register.Request request, string erroredValidationProperty)
    {
        var response = await context.HttpClient.PostAsJsonAsync("/api/auth/register", request);
        
        var problemResult = await response.Content.ReadFromJsonAsync<HttpValidationProblemDetails>();
        
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.NotNull(problemResult?.Errors);
        Assert.Contains(problemResult.Errors, (error) => error.Key == erroredValidationProperty);
    }

    [Theory, MemberData(nameof(InvalidLogins))]
    public async Task TestInvalidLoginValidation(Login.Request request, string erroredValidationProperty)
    {
        var response = await context.HttpClient.PostAsJsonAsync("/api/auth/login", request);
        
        var problemResults = await response.Content.ReadFromJsonAsync<HttpValidationProblemDetails>();

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.NotNull(problemResults);
        Assert.Contains(problemResults.Errors, error => error.Key == erroredValidationProperty);
    }
    
    [Fact]
    public async Task TestSuccessfulRegistration()
    {
        var uniqueEmail = $"{Guid.NewGuid()}@email.com";
        var request = new Register.Request(uniqueEmail, "ValidPass1", "ValidPass1");
        
        var response = await context.HttpClient.PostAsJsonAsync("/api/auth/register", request);
        var userResult = await response.Content.ReadFromJsonAsync<Register.Response>();
        
        var dbUser = await context.Db.User.SingleOrDefaultAsync(x => x.Username == userResult.UserResponse.Username);
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(userResult);
        Assert.NotNull(dbUser);
        Assert.NotEmpty(dbUser.Password);
        
        context.Db.Remove(dbUser);
        await context.Db.SaveChangesAsync();
    }
}