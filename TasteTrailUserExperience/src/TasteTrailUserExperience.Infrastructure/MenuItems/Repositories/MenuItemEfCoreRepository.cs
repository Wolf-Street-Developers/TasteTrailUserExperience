using Microsoft.EntityFrameworkCore;
using TasteTrailData.Core.Filters.Specifications;
using TasteTrailUserExperience.Core.MenuItems.Models;
using TasteTrailUserExperience.Core.MenuItems.Repositories;
using TasteTrailUserExperience.Infrastructure.Common.Data;

namespace TasteTrailUserExperience.Infrastructure.MenuItems.Repositories;

public class MenuItemEfCoreRepository : IMenuItemRepository
{
    private readonly UserExperienceDbContext _dbContext;

    public MenuItemEfCoreRepository(UserExperienceDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<List<MenuItem>> GetFilteredByIdAsync(FilterParameters<MenuItem> parameters, int menuId)
    {
        IQueryable<MenuItem> query = _dbContext.Set<MenuItem>();

        query = query.Where(mi => mi.MenuId == menuId);

        if (parameters.Specification is not null)
            query = parameters.Specification.Apply(query);


        if (parameters.SearchTerm is not null)
        {
            var searchTerm = $"%{parameters.SearchTerm.ToLower()}%";

            query = query.Where(mi =>
                (mi.Name != null && EF.Functions.Like(mi.Name.ToLower(), searchTerm)) ||
                (mi.Description != null && EF.Functions.Like(mi.Description.ToLower(), searchTerm))
            );
        }
        
        query = query.Skip((parameters.PageNumber - 1) * parameters.PageSize).Take(parameters.PageSize);

        return await query.ToListAsync();
    }

    public async Task<List<MenuItem>> GetFilteredAsync(FilterParameters<MenuItem> parameters)
    {
        IQueryable<MenuItem> query = _dbContext.Set<MenuItem>();

        if (parameters.Specification is not null)
            query = parameters.Specification.Apply(query);


        if (parameters.SearchTerm is not null)
        {
            var searchTerm = $"%{parameters.SearchTerm.ToLower()}%";

            query = query.Where(mi =>
                (mi.Name != null && EF.Functions.Like(mi.Name.ToLower(), searchTerm)) ||
                (mi.Description != null && EF.Functions.Like(mi.Description.ToLower(), searchTerm))
            );
        }  
        
        query = query.Skip((parameters.PageNumber - 1) * parameters.PageSize).Take(parameters.PageSize);

        return await query.ToListAsync();
    }

    public async Task<int> GetCountFilteredIdAsync(FilterParameters<MenuItem>? parameters, int menuId)
    {
        var query = _dbContext.MenuItems.AsQueryable();
        query = query.Where(mi => mi.MenuId == menuId);

        if (parameters is null)
            return await query.CountAsync();

        if (parameters.Specification != null)
            query = parameters.Specification.Apply(query);

        return await query.CountAsync();
    }

    public async Task<int> GetCountFilteredAsync(FilterParameters<MenuItem>? parameters)
    {
        var query = _dbContext.MenuItems.AsQueryable();

        if (parameters is null)
            return await query.CountAsync();

        if (parameters.Specification != null)
            query = parameters.Specification.Apply(query);

        return await query.CountAsync();
    }

    public async Task<MenuItem?> GetByIdAsync(int id)
    {
        return await _dbContext.MenuItems
            .FirstOrDefaultAsync(mi => mi.Id == id);
    }

    public async Task<int> CreateAsync(MenuItem menuItem)
    {
        ArgumentNullException.ThrowIfNull(menuItem);

        var menu = await _dbContext.Menus.FirstOrDefaultAsync(m => m.Id == menuItem.MenuId) ?? 
            throw new ArgumentException($"Menu by ID: {menuItem.MenuId} not found.");

        await _dbContext.MenuItems.AddAsync(menuItem);
        await _dbContext.SaveChangesAsync();

        return menuItem.Id;
    }

    public async Task<int?> DeleteByIdAsync(int id)
    {
        var menuItem = await _dbContext.MenuItems.FindAsync(id);

        if (menuItem is null)
            return null;
        
        _dbContext.MenuItems.Remove(menuItem);
        await _dbContext.SaveChangesAsync();

        return id;
    }

    public async Task<int?> PutAsync(MenuItem menuItem)
    {
        var menuItemToUpdate = await _dbContext.MenuItems
            .FirstOrDefaultAsync(mi => mi.Id == menuItem.Id);

        if (menuItemToUpdate is null)
            return null;

        
        menuItemToUpdate.Name = menuItem.Name;
        menuItemToUpdate.Description = menuItem.Description;
        menuItemToUpdate.Price = menuItem.Price;
        menuItemToUpdate.ImageUrlPath = menuItem.ImageUrlPath;

        await _dbContext.SaveChangesAsync();

        return menuItem.Id;
    }

    public async Task<MenuItem?> GetAsNoTrackingAsync(int id)
    {
        return await _dbContext.MenuItems
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task<int?> IncrementLikesAsync(MenuItem menuItem)
    {
        var menuItemToUpdate = await _dbContext.MenuItems
            .FirstOrDefaultAsync(mi => mi.Id == menuItem.Id);

        if (menuItemToUpdate is null)
            return null;

        
        menuItemToUpdate.Likes++;

        await _dbContext.SaveChangesAsync();

        return menuItem.Id;
    }

    public async Task<int?> DecrementLikesAsync(MenuItem menuItem)
    {
        var menuItemToUpdate = await _dbContext.MenuItems
            .FirstOrDefaultAsync(mi => mi.Id == menuItem.Id);

        if (menuItemToUpdate is null)
            return null;

        
        menuItemToUpdate.Likes--;

        await _dbContext.SaveChangesAsync();

        return menuItem.Id;
    }
}
