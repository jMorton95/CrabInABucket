using FinanceManager.Common.Constants;
using FinanceManager.Common.Contracts;
using FinanceManager.Common.Entities;
using FinanceManager.Common.Models;
using FinanceManager.Common.Services;
using FinanceManager.Data;
using Microsoft.EntityFrameworkCore;
using Range = FinanceManager.Common.Models.Range;

namespace FinanceManager.Simulation.Generation;

public class Simulator(DataContext db, IPasswordHasher passwordHasher) : ISimulator
{
    private static int GetNumberFromRange(Range range) 
        => new Random().Next(range.Min, range.Max);
    
    private async Task<List<User>> CreateUsers(SimulationParameters simulationParameters)
    {
        var numberOfUsersToSimulate = GetNumberFromRange(simulationParameters.Users.Count);
        var dummyPassword = passwordHasher.HashPassword(TestConstants.Password);

        for (var i = numberOfUsersToSimulate; i > 0; i--)
        {
            var userName = Faker.Internet.Email(Faker.Name.FullName());

            User user = new() { Username = userName, Password = dummyPassword, WasSimulated = true};

            await db.AddAsync(user);
        }

        await db.SaveChangesAsync();

        var seededUsers = await db.User
            .OrderByDescending(x => x.Id)
            .Where(y => y.WasSimulated)
            .Take(numberOfUsersToSimulate)
            .ToListAsync();
        
        return seededUsers;
    }

    // private async Task<bool> BuildUserAccounts()
    // {
    //     
    // }

    public async Task<bool> RunSimulation(SimulationParameters simulationParameters)
    {
        var simUsersResult = await CreateUsers(simulationParameters);

        return new List<int>([simUsersResult.Count]).TrueForAll(x => x > 0);
    }
}