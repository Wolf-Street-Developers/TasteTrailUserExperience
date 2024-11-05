using Microsoft.EntityFrameworkCore;
using TasteTrailData.Core.Filters.Specifications;
using TasteTrailUserExperience.Core.Menus.Models;
using TasteTrailUserExperience.Core.Menus.Repositories;
using TasteTrailUserExperience.Infrastructure.Common.Data;

namespace TasteTrailUserExperience.Infrastructure.Menus.Repositories;

public class MenuEfCoreRepository : IMenuRepository
{
    private readonly UserExperienceDbContext _dbContext;

    public MenuEfCoreRepository(UserExperienceDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<List<Menu>> GetFilteredByIdAsync(FilterParameters<Menu> parameters, int venueId)
    {
        IQueryable<Menu> query = _dbContext.Set<Menu>();

        query = query.Where(m => m.VenueId == venueId);
        query = query.Skip((parameters.PageNumber - 1) * parameters.PageSize).Take(parameters.PageSize);

        return await query.ToListAsync();
    }

    
    public async Task<List<Menu>> GetFilteredAsync(FilterParameters<Menu> parameters)
    {
        IQueryable<Menu> query = _dbContext.Set<Menu>();

        if (parameters.SearchTerm is not null)
        {
            var searchTerm = $"%{parameters.SearchTerm.ToLower()}%";

            query = query.Where(m =>
                (m.Name != null && EF.Functions.Like(m.Name.ToLower(), searchTerm)) ||
                (m.Description != null && EF.Functions.Like(m.Description.ToLower(), searchTerm))
            );
        }

        query = query.Skip((parameters.PageNumber - 1) * parameters.PageSize).Take(parameters.PageSize);

        return await query.ToListAsync();
    }

    public async Task<Menu?> GetByIdAsync(int id)
    {
        return await _dbContext.Menus
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<int> GetCountFilteredIdAsync(FilterParameters<Menu>? parameters, int venueId)
    {
        var query = _dbContext.Menus.AsQueryable();
        query = query.Where(m => m.VenueId == venueId);

        if (parameters is null)
            return await query.CountAsync();

        if (parameters.Specification != null)
            query = parameters.Specification.Apply(query);

        return await query.CountAsync();
    }
    
    public async Task<int> GetCountFilteredAsync(FilterParameters<Menu>? parameters)
    {
        var query = _dbContext.Menus.AsQueryable();

        if (parameters is null)
            return await query.CountAsync();

        if (parameters.Specification != null)
            query = parameters.Specification.Apply(query);

        return await query.CountAsync();
    }

    public async Task<int> CreateAsync(Menu menu)
    {
        ArgumentNullException.ThrowIfNull(menu);

        await _dbContext.Menus.AddAsync(menu);
        await _dbContext.SaveChangesAsync();

        return menu.Id;
    }

    public async Task<int?> DeleteByIdAsync(int id)
    {
        var menu = await _dbContext.Menus.FindAsync(id);

        if (menu is null)
            return null;
        
        _dbContext.Menus.Remove(menu);
        await _dbContext.SaveChangesAsync();

        return id;
    }

    public async Task<int?> PutAsync(Menu menu)
    {
        var menuToUpdate = await _dbContext.Menus
            .FirstOrDefaultAsync(m => m.Id == menu.Id);

        if (menuToUpdate is null)
            return null;

        menuToUpdate.Name = menu.Name;
        menuToUpdate.Description = menu.Description;

        await _dbContext.SaveChangesAsync();

        return menu.Id;
    }
}
