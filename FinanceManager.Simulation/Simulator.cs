﻿using EFCore.BulkExtensions;
using FinanceManager.Common.Constants;
using FinanceManager.Common.Contracts;
using FinanceManager.Common.Entities;
using FinanceManager.Common.Models;
using FinanceManager.Common.Services;
using FinanceManager.Data;
using FinanceManager.Data.Read.Friends;
using FinanceManager.Data.Seeding;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Simulation;

public class Simulator(
    SimulationParameters simulationParameters,
    DataContext db,
    IDbContextFactory<DataContext> dbContextFactory,
    IPasswordHasher passwordHasher,
    ISimulationPlanBuilder simulationPlanBuilder,
    IReadUserFriends readUserFriends
    ) : ISimulator
{
    private readonly SimulationPlan _simulationPlan = simulationPlanBuilder.CreateSimulationPlan(simulationParameters);
    private async Task<List<User>> CreateUsers(DateTime tickDate)
    {
        var dummyPassword = passwordHasher.HashPassword(TestConstants.Password);

        var usersToSeed = Enumerable.Range(1, _simulationPlan.UsersPerTick).Select(index =>
        {
            var userName = Faker.Internet.Email($"{Faker.Name.First()}.{index}.{Faker.Name.Last()}.{index}");

            User user = new() { Username = userName, Password = dummyPassword, WasSimulated = true, CreatedDate = tickDate, UpdatedDate = tickDate };

            return user;
        });

        await db.AddRangeAsync(usersToSeed.ToList());
        await db.BulkSaveChangesAsync();
        
        var seededUsers = await db.User
            .OrderByDescending(x => x.Id)
            .Where(y => y.WasSimulated)
            .Take(_simulationPlan.UsersPerTick)
            .ToListAsync();
        
        return seededUsers;
    }

    private async Task<int> CreateAccounts(IEnumerable<User> users, DateTime tickDate)
    {
        var accounts = users.SelectMany(user =>
        {
            return Enumerable.Range(1, SimulationHelpers.GetNumberFromRange(simulationParameters.Accounts.Count))
                .Select(_ => new Account
                    {
                        Balance = SimulationHelpers.GetWeightedRandomNumber(1, simulationParameters.Accounts.MaxStartingBalance, simulationParameters.Accounts.StartingBalanceBias),
                        User = user,
                        Name = Faker.Company.Name(),
                        CreatedDate = tickDate,
                        UpdatedDate = tickDate,
                        WasSimulated = true
                    }
                );
        }).ToList();

        await db.AddRangeAsync(accounts);
        await db.BulkSaveChangesAsync();

        return 1;
    }
    
    private async Task<int> CreateFriendships(DateTime tickDate)
    {
        var usersWithLessThanMaxFriends = await db.User
            .Include(user => user.UserFriendships)
            .Where(x => x.WasSimulated && 
                        x.UserFriendships.Count <= _simulationPlan.MaxFriendsPerUser)
            .ToListAsync();
        
        if (usersWithLessThanMaxFriends is {Count: 0})
        {
            return 1;
        }

        List<Friendship> newFriendShips = [];
        List<UserFriendship> newUserFriendShips = [];

        
        var tasks = usersWithLessThanMaxFriends.Select(async user =>
        {
            await using var taskDbContext = await dbContextFactory.CreateDbContextAsync();
            var newFriends = await taskDbContext.StaticGetRandomSuggestions(user.Id, _simulationPlan.MaxFriendsPerTick);
        
            if (newFriends.Count <= 0)
            {
                return;
            }
        
            var friendships = newFriends.Select(_ => new Friendship
            {
                CreatedDate = tickDate,
                UpdatedDate = tickDate,
                WasSimulated = true,
                IsAccepted = true,
                IsPending = false
            }).ToList();
            
            var userFriendships = friendships.SelectMany((friendship, index) =>
            {
                var userUf = new UserFriendship
                    { UserId = user.Id, Friendship = friendship, WasSimulated = true, CreatedDate = tickDate, UpdatedDate = tickDate };
                var newFriendUf = new UserFriendship
                    { UserId = newFriends[index].Id, Friendship = friendship, WasSimulated = true, CreatedDate = tickDate, UpdatedDate = tickDate };
        
                return new List<UserFriendship> { userUf, newFriendUf };
            }).ToList();
            
            newFriendShips.AddRange(friendships);
            newUserFriendShips.AddRange(userFriendships);
        }).ToList();
        
        await Task.WhenAll(tasks);
        
        await db.AddRangeAsync(newFriendShips);
        await db.AddRangeAsync(newUserFriendShips);
        
        await db.BulkSaveChangesAsync();
        
        var stuff = await db.StatsicGetRandomSuggestions(usersWithLessThanMaxFriends.Select(x => x.Id).ToList(), 10);
        var thing = 1;

        return 1;
    }

    private async Task<bool> ProcessSimulationTick(int tickNumber)
    {
        var tickDate = DateTime.UtcNow.AddMonths(tickNumber);
        
        var simUsersResult = await CreateUsers(tickDate);
        var simAccountsResults = await CreateAccounts(simUsersResult, tickDate);
        var simFriendsResults = await CreateFriendships(tickDate);
        
        List<int> results = [simUsersResult.Count, simAccountsResults, simFriendsResults];

        return results.TrueForAll(x => x > 0);
    }
    
    private async Task<bool> RemoveSimulatedData(Settings settings)
    {
        await db.User.Where(x => x.WasSimulated).ExecuteDeleteAsync();
        await db.Account.Where(x => x.WasSimulated).ExecuteDeleteAsync();
        await db.Friendship.Where(x => x.WasSimulated).ExecuteDeleteAsync();
        await db.UserFriendship.Where(x => x.WasSimulated).ExecuteDeleteAsync();
        
        settings.HasBeenSimulated = false;
        db.Settings.Update(settings);
        
        return await db.SaveChangesAsync() > 0;
    }
    
    public async Task<bool> StartSimulation(Settings settings)
    {
        Dictionary<int, bool> tickResults = [];
        
        settings.HasBeenSimulated = true;
        db.Settings.Update(settings);
        await db.SaveChangesAsync();
        
        foreach (var tick in Enumerable.Range(1, simulationParameters.Duration))
        {
            tickResults.Add(tick, await ProcessSimulationTick(tick));
        }

        if (simulationParameters.RemoveDataIfError && !tickResults.Values.ToList().TrueForAll(x => x))
        {
            var clearDownResult = await RemoveSimulatedData(settings);
            return clearDownResult;
        };
        
        return true;
    }
    
    public async Task<bool> SimulateFromConfiguration()
    {
        var settings = await db.ConfigureSettingsTable(simulationParameters.ShouldSimulate, simulationParameters.ShouldOverwrite);

        return settings switch
        {
            { HasBeenSimulated: false, ShouldSimulate: true } => await StartSimulation(settings),
            { ShouldSimulate: true, ShouldOverwrite: true} => await OverwriteAndSimulate(),
            { ShouldSimulate: false } => await RemoveSimulatedData(settings),
            _ => false
        };
        
        async Task<bool> OverwriteAndSimulate()
        {
            await RemoveSimulatedData(settings);
            return await StartSimulation(settings);
        }
    }
}