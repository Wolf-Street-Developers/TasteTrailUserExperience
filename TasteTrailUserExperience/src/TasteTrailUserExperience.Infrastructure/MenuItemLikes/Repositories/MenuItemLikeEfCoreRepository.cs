using Microsoft.EntityFrameworkCore;
using TasteTrailUserExperience.Core.MenuItemLikes.Models;
using TasteTrailUserExperience.Core.MenuItemLikes.Repositories;
using TasteTrailUserExperience.Infrastructure.Common.Data;

namespace TasteTrailUserExperience.Infrastructure.MenuItemLikes.Repositories;

public class MenuItemLikeEfCoreRepository : IMenuItemLikeRepository
{
    private readonly UserExperienceDbContext _dbContext;

    public MenuItemLikeEfCoreRepository(UserExperienceDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<int> CreateAsync(MenuItemLike menuItemLike)
    {
        ArgumentNullException.ThrowIfNull(menuItemLike);

        var menuItem = await _dbContext.MenuItems.FirstOrDefaultAsync(m => m.Id == menuItemLike.MenuItemId) ?? 
            throw new ArgumentException($"MenuItem by ID: {menuItemLike.MenuItemId} not found.");

        await _dbContext.MenuItemLikes.AddAsync(menuItemLike);
        await _dbContext.SaveChangesAsync();

        return menuItem.Id;
    }

    public async Task<int?> DeleteByIdAsync(int id)
    {
        var menuItemLike = await _dbContext.MenuItemLikes.FindAsync(id);

        if (menuItemLike is null)
            return null;
        
        _dbContext.MenuItemLikes.Remove(menuItemLike);
        await _dbContext.SaveChangesAsync();

        return id;
    }

    public async Task<MenuItemLike?> GetAsNoTrackingAsync(int id)
    {
        return await _dbContext.MenuItemLikes
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task<List<int>> GetLikedMenuItemIds(string userId)
    {
        return await _dbContext.MenuItemLikes
                             .Where(ml => ml.UserId == userId)
                             .Select(ml => ml.MenuItemId)
                             .ToListAsync();
    }

    public async Task<bool> Exists(int menuItemId, string userId)
    {
        return await _dbContext.MenuItemLikes
                             .AnyAsync(ml => ml.UserId == userId && ml.MenuItemId == menuItemId);
    }

    public async Task<MenuItemLike?> GetByMenuItemAndUserId(int menuItemId, string userId)
    {
        return  await _dbContext.MenuItemLikes
                    .FirstOrDefaultAsync(ml => ml.UserId == userId && ml.MenuItemId == menuItemId);
    }
}
