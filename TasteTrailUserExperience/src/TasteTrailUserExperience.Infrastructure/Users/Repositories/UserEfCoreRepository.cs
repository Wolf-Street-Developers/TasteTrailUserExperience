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

    public Task<User?> GetByIdAsync(string? id)
    {
        return _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
    }
}
