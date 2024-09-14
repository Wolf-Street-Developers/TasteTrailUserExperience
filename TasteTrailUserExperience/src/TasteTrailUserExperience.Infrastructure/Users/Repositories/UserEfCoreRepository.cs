using Microsoft.EntityFrameworkCore;
using TasteTrailUserExperience.Core.Users.Models;
using TasteTrailUserExperience.Core.Users.Repositories;
using TasteTrailUserExperience.Infrastructure.Common.Data;

namespace TasteTrailUserExperience.Infrastructure.Users.Repositories;

public class UserEfCoreRepository : IUserRepository
{
    private readonly UserExperienceDbContext _dbContext;

    public UserEfCoreRepository(UserExperienceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> GetByIdAsync(string? id)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<string> CreateAsync(User user)
    {
        ArgumentNullException.ThrowIfNull(user);

        var existingUser = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == user.Id);

        if (existingUser != null)
            return existingUser.Id;
            
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();

        return user.Id;
    }

    public async Task<string?> PutAsync(User user)
    {
        ArgumentNullException.ThrowIfNull(user);

        var userToUpdate = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == user.Id);

        if (userToUpdate is null)
            return null;

        userToUpdate.Username = user.Username;
        await _dbContext.SaveChangesAsync();

        return user.Id;
    }
}
