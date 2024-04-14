using FinanceManager.Common.Constants;
using FinanceManager.Common.Contracts;
using FinanceManager.Common.Entities;
using FinanceManager.Common.Models;
using FinanceManager.Common.Services;
using FinanceManager.Data;
using FinanceManager.Data.Seeding;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Simulation;

public class Simulator(SimulationParameters simulationParameters, DataContext db, IPasswordHasher passwordHasher, ISimulationPlanBuilder simulationPlanBuilder) : ISimulator
{
    private readonly SimulationPlan _simulationPlan = simulationPlanBuilder.CreateSimulationPlan(simulationParameters);
    private async Task<List<User>> CreateUsers(int numberOfUsersPerTick, DateTime tickDate)
    {
        var dummyPassword = passwordHasher.HashPassword(TestConstants.Password);

        var usersToSeed = Enumerable.Range(1, numberOfUsersPerTick).Select(x =>
        {
            var userName = Faker.Internet.Email(Faker.Name.First());

            User user = new() { Username = userName, Password = dummyPassword, WasSimulated = true, CreatedDate = tickDate, UpdatedDate = tickDate };

            return user;
        });

        await db.AddRangeAsync(usersToSeed.ToList());
        await db.SaveChangesAsync();

        var seededUsers = await db.User
            .OrderByDescending(x => x.Id)
            .Where(y => y.WasSimulated)
            .Take(numberOfUsersPerTick)
            .ToListAsync();
        
        return seededUsers;
    }

    // private async Task<bool> BuildUserAccounts(IEnumerable<User> users, DateTime tickDate)
    // {
    //     
    // }

    private async Task<bool> ProcessSimulationTick(int tickNumber)
    {
        var tickDate = DateTime.UtcNow.AddMonths(tickNumber);
        
        var simUsersResult = await CreateUsers(_simulationPlan.UsersPerTick, tickDate);
        
        List<int> results = [simUsersResult.Count];

        return results.TrueForAll(x => x > 0);
    }

    public async Task<bool> StartSimulation(Settings settings)
    {
        Dictionary<int, bool> tickResults = [];
        
        foreach (var tick in Enumerable.Range(1, simulationParameters.Duration))
        {
            tickResults.Add(tick, await ProcessSimulationTick(tick));
        }

        if (!tickResults.Values.ToList().TrueForAll(x => x))
        {
            //TODO: Add Partial as a parameter, only remove if partial is false
            await RemoveSimulatedData(settings);
            return false;
        };
        
        settings.HasBeenSimulated = true;
        db.Settings.Update(settings);     
        
        return await db.SaveChangesAsync() > 0;
    }

    public async Task<bool> RemoveSimulatedData(Settings settings)
    {
        await db.User.Where(x => x.WasSimulated).ExecuteDeleteAsync();
        
        settings.HasBeenSimulated = false;
        db.Settings.Update(settings);
        
        return await db.SaveChangesAsync() > 0;
    }
    
    public async Task<bool> SimulateFromConfiguration()
    {
        var settings = await db.ConfigureSettingsTable(simulationParameters.ShouldSimulate);

        return settings switch
        {
            { HasBeenSimulated: false, ShouldSimulate: true } => await StartSimulation(settings),
            { HasBeenSimulated: true, ShouldSimulate: false } => await RemoveSimulatedData(settings),
            _ => false
        };
    }
}