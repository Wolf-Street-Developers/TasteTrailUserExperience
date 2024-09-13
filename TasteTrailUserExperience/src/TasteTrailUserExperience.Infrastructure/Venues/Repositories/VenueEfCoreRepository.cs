using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using TasteTrailData.Core.Filters.Specifications;
using TasteTrailUserExperience.Core.Venues.Models;
using TasteTrailUserExperience.Core.Venues.Repositories;
using TasteTrailUserExperience.Infrastructure.Common.Data;

namespace TasteTrailUserExperience.Infrastructure.Venues.Repositories;

public class VenueEfCoreRepository : IVenueRepository
{
    private readonly UserExperienceDbContext _dbContext;

    public VenueEfCoreRepository(UserExperienceDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<List<Venue>> GetFilteredAsync(FilterParameters<Venue> parameters)
    {
        IQueryable<Venue> query = _dbContext.Set<Venue>();

        if (parameters.Specification is not null)
            query = parameters.Specification.Apply(query);

        if (parameters.SearchTerm is not null)
        {
            var searchTerm = $"%{parameters.SearchTerm.ToLower()}%";

            query = query.Where(v =>
                (v.Name != null && EF.Functions.Like(v.Name.ToLower(), searchTerm)) ||
                (v.Description != null && EF.Functions.Like(v.Description.ToLower(), searchTerm))
            );
        }

        query = query.Skip((parameters.PageNumber - 1) * parameters.PageSize).Take(parameters.PageSize);

        return await query.ToListAsync();
    }

    public async Task<Venue?> GetByIdAsync(int id)
    {
        return await _dbContext.Venues
            .FirstOrDefaultAsync(v => v.Id == id);
    }

    public async Task<Venue?> GetAsNoTrackingAsync(int id)
    {
        return await _dbContext.Venues
            .AsNoTracking()
            .FirstOrDefaultAsync(v => v.Id == id);
    }

    public async Task<int> GetCountFilteredAsync(FilterParameters<Venue>? parameters)
    {
        var query = _dbContext.Venues.AsQueryable();

        if (parameters is null)
            return await query.CountAsync();

        if (parameters.Specification is not null)
            query = parameters.Specification.Apply(query);

        if (parameters.SearchTerm is not null)
        {
            var searchTerm = $"%{parameters.SearchTerm.ToLower()}%";

            query = query.Where(v =>
                (v.Name != null && EF.Functions.Like(v.Name.ToLower(), searchTerm)) ||
                (v.Description != null && EF.Functions.Like(v.Description.ToLower(), searchTerm))
            );
        }

        return await query.CountAsync();
    }

    public async Task<int> CreateAsync(Venue venue)
    {
        ArgumentNullException.ThrowIfNull(venue);

        await _dbContext.Venues.AddAsync(venue);
        await _dbContext.SaveChangesAsync();

        System.Console.WriteLine("TEST");

        return venue.Id;
    }

    public async Task<int?> DeleteByIdAsync(int id)
    {
        var venue = await _dbContext.Venues.FindAsync(id);

        if (venue is null)
            return null;
        
        _dbContext.Venues.Remove(venue);
        await _dbContext.SaveChangesAsync();

        return id;
    }

    public async Task<int?> PutAsync(Venue venue)
    {
        ArgumentNullException.ThrowIfNull(venue);

        var venueToUpdate = await _dbContext.Venues
            .FirstOrDefaultAsync(v => v.Id == venue.Id);

        if (venueToUpdate is null)
            return null;

        venueToUpdate.Name = venue.Name;
        venueToUpdate.Latitude = venue.Latitude;
        venueToUpdate.Longtitude = venue.Longtitude;
        venueToUpdate.LogoUrlPath = venue.LogoUrlPath;
        venueToUpdate.Address = venue.Address;
        venueToUpdate.ContactNumber = venue.ContactNumber;
        venueToUpdate.Email = venue.Email;
        venueToUpdate.AveragePrice = venue.AveragePrice;

        await _dbContext.SaveChangesAsync();

        return venueToUpdate.Id;
    }
}
