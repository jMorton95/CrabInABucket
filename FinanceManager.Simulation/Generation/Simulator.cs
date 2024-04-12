using FinanceManager.Common.Constants;
using FinanceManager.Common.Entities;
using FinanceManager.Common.Services;
using FinanceManager.Data;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Simulation.Generation;

public interface ISimulator
{
    Task<bool> RunSimulation(Parameters parameters);
}

public class Simulator(DataContext db, IPasswordHasher passwordHasher) : ISimulator
{
    private static int GetNumberFromRange(Range range) 
        => new Random().Next(range.Min, range.Max);
    
    private async Task<List<User>> CreateUsers(Parameters parameters)
    {
        var numberOfUsersToSeed = GetNumberFromRange(parameters.Users.Count);
        var dummyPassword = passwordHasher.HashPassword(TestConstants.Password);

        for (var i = numberOfUsersToSeed; i > 0; i--)
        {
            var userName = Faker.Internet.Email(Faker.Name.FullName());

            User user = new() { Username = userName, Password = dummyPassword};

            await db.AddAsync(user);
        }

        await db.SaveChangesAsync();

        var seededUsers = await db.User
            .OrderByDescending(x => x.Id)
            .Take(numberOfUsersToSeed)
            .ToListAsync();
        
        return seededUsers;
    }

    // private async Task<bool> BuildUserAccounts()
    // {
    //     
    // }

    public async Task<bool> RunSimulation(Parameters parameters)
    {
        var simUsersResult = await CreateUsers(parameters);

        return new List<int>([simUsersResult.Count]).TrueForAll(x => x > 0);
    }
}