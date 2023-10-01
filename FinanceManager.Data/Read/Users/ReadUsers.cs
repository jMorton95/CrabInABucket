using FinanceManager.Core.Models;
using FinanceManager.Data.Read.Generic;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Data.Read.Users;

public interface IReadUsers : IRead<User>
{
    Task<bool> CheckUserExistsByEmail(string emailAddress);
}

public sealed class ReadUsers(DataContext db) : IReadUsers
{
    public async Task<IEnumerable<User>> GetAllAsync()
        => await db.User.Include(x => x.Roles).ToListAsync();
    
    public async Task<User?> GetByIdAsync(Guid id)
        => await db.User.FirstOrDefaultAsync(x => x.Id == id);

    public async Task<bool> CheckUserExistsByEmail(string emailAddress)
    {
        return await db.User.AnyAsync(x => x.Username == emailAddress);
    }
}