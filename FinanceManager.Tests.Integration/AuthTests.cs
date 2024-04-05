using System.Net;
using FinanceManager.Api.Endpoints.Auth;
using FinanceManager.Common.Entities;
using FinanceManager.Tests.Integration.Setup;
using Microsoft.EntityFrameworkCore;

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

    public static List<object[]> InvalidLogins =>
    [
    ];

    public static List<object[]> ExistingUser => [[new Register.Request(TestConstants.Username, TestConstants.Password, TestConstants.Password), "Username"]];
    
    [Theory, MemberData(nameof(InvalidRegistrations))]
    public async Task TestRegistrationValidation(Register.Request request, string validationParameterError)
    {
        var response = await HttpClient.PostAsJsonAsync("/api/auth/register", request);
        
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        
        var problemResult = await response.Content.ReadFromJsonAsync<HttpValidationProblemDetails>();
        
        Assert.NotNull(problemResult?.Errors);
        Assert.Contains(problemResult.Errors, (error) => error.Key == validationParameterError);
    }

    [Theory, MemberData(nameof(ExistingUser))]
    public async Task TestExistingUserValidation(Register.Request request, string validationParameterError)
    {
        var existingUser = new User() { Username = request.Username, Password = request.Password };
        DataContext.User.Add(existingUser);
        await DataContext.SaveChangesAsync();
        
        var response = await HttpClient.PostAsJsonAsync("/api/auth/register", request);
        
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        
        var problemResult = await response.Content.ReadFromJsonAsync<HttpValidationProblemDetails>();
        
        Assert.NotNull(problemResult?.Errors);
        Assert.Contains(problemResult.Errors, (error) => error.Key == validationParameterError);
    }

    [Fact]
    public async Task TestSuccessfulRegistration()
    {
        await AuthContext.ConfigureAuthenticationContext();
        var uniqueEmail = $"{Guid.NewGuid()}@email.com";
        var request = new Register.Request(uniqueEmail, "ValidPass1", "ValidPass1");
        
        var response = await HttpClient.PostAsJsonAsync("/api/auth/register", request);
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var userResult = await response.Content.ReadFromJsonAsync<Register.Response>();

        Assert.NotNull(userResult);

        var dbUser = await DataContext.User.SingleOrDefaultAsync(x => x.Username == userResult.UserResponse.Username);
        
        Assert.NotNull(dbUser);
        Assert.NotEmpty(dbUser.Password);
        
        if (dbUser != null)
        {
            DataContext.Remove(dbUser);
            await DataContext.SaveChangesAsync();
        }
    }
}