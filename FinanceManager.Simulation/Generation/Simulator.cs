using FinanceManager.Common.Constants;
using FinanceManager.Common.Contracts;
using FinanceManager.Common.Entities;
using FinanceManager.Common.Models;
using FinanceManager.Common.Services;
using FinanceManager.Data;
using FinanceManager.Data.Seeding;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Simulation.Generation;

public class Simulator(DataContext db, IPasswordHasher passwordHasher, ISimulationPlanBuilder simulationPlanBuilder) : ISimulator
{
    private async Task<List<User>> CreateUsers(int numberOfUsersPerTick)
    {
        var dummyPassword = passwordHasher.HashPassword(TestConstants.Password);

        for (var i = numberOfUsersPerTick; i > 0; i--)
        {
            var userName = Faker.Internet.Email(Faker.Name.First());

            User user = new() { Username = userName, Password = dummyPassword, WasSimulated = true};

            await db.AddAsync(user);
        }

        await db.SaveChangesAsync();

        var seededUsers = await db.User
            .OrderByDescending(x => x.Id)
            .Where(y => y.WasSimulated)
            .Take(numberOfUsersPerTick)
            .ToListAsync();
        
        return seededUsers;
    }

    // private async Task<bool> BuildUserAccounts()
    // {
    //     
    // }

    private async Task<bool> ProcessSimulationTick(SimulationParameters simulationParameters, SimulationPlan simulationPlan, int tickNumber)
    {
        var simUsersResult = await CreateUsers(simulationPlan.UsersPerTick);
        
        List<int> results = [simUsersResult.Count];

        if (!results.TrueForAll(x => x > 0))
        {
            return false;
        }
    }

    public async Task<bool> StartSimulation(SimulationParameters simulationParameters, Settings settings)
    {
        var simulationPlan = await simulationPlanBuilder.CreateSimulationPlan(simulationParameters);

        Dictionary<int, bool> tickResults = [];
        
        foreach (var tick in Enumerable.Range(0, simulationParameters.Duration))
        {
            tickResults.Add(tick, await ProcessSimulationTick(simulationParameters, simulationPlan, tick));
        }

        if (!tickResults.Values.ToList().TrueForAll(x => x))
        {
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
        
        return await Task.FromResult(true);
    }
    
    public async Task<bool> SimulateFromConfiguration(SimulationParameters simulationParameters)
    {
        var settings = await db.ConfigureSettingsTable(simulationParameters.ShouldSimulate);

        return settings switch
        {
            { HasBeenSimulated: false, ShouldSimulate: true } => await StartSimulation(simulationParameters, settings),
            { HasBeenSimulated: true, ShouldSimulate: false } => await RemoveSimulatedData(settings),
            _ => false
        };
    }
}