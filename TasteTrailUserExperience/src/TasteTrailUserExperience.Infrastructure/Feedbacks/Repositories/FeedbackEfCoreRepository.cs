using Microsoft.EntityFrameworkCore;
using TasteTrailData.Core.Filters.Specifications;
using TasteTrailUserExperience.Core.Feedbacks.Models;
using TasteTrailUserExperience.Core.Feedbacks.Repositories;
using TasteTrailUserExperience.Infrastructure.Common.Data;

namespace TasteTrailUserExperience.Infrastructure.Feedbacks.Repositories;

public class FeedbackEfCoreRepository : IFeedbackRepository
{
    private readonly UserExperienceDbContext _dbContext;

    public FeedbackEfCoreRepository(UserExperienceDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<List<Feedback>> GetFilteredByIdAsync(FilterParameters<Feedback> parameters, int venueId)
    {
        IQueryable<Feedback> query = _dbContext.Set<Feedback>();

        query = query.Where(f => f.VenueId == venueId); // Getting feedbacks by VenueId

        if (parameters.Specification is not null)
            query = parameters.Specification.Apply(query); // Adding Filter

        query = query.Skip((parameters.PageNumber - 1) * parameters.PageSize).Take(parameters.PageSize); // Applying pagination

        return await query.ToListAsync();
    }

    
    public async Task<List<Feedback>> GetFilteredAsync(FilterParameters<Feedback> parameters)
    {
        IQueryable<Feedback> query = _dbContext.Set<Feedback>();

        if (parameters.Specification is not null)
            query = parameters.Specification.Apply(query); // Adding Filter

        if (parameters.SearchTerm is not null)
        {
            var searchTerm = $"%{parameters.SearchTerm.ToLower()}%";

            query = query.Where(f =>
                f.Text != null && EF.Functions.Like(f.Text.ToLower(), searchTerm)
            );
        }

        query = query.Skip((parameters.PageNumber - 1) * parameters.PageSize).Take(parameters.PageSize); // Applying pagination

        return await query.ToListAsync();
    }

    public async Task<int> GetCountFilteredIdAsync(FilterParameters<Feedback>? parameters, int venueId)
    {
        var query = _dbContext.Feedbacks.AsQueryable();
        query = query.Where(f => f.VenueId == venueId);

        if (parameters is null)
            return await query.CountAsync();

        if (parameters.Specification != null)
            query = parameters.Specification.Apply(query);

        return await query.CountAsync();
    }

    public async Task<int> GetCountFilteredAsync(FilterParameters<Feedback>? parameters)
    {
        var query = _dbContext.Feedbacks.AsQueryable();

        if (parameters is null)
            return await query.CountAsync();

        if (parameters.Specification != null)
            query = parameters.Specification.Apply(query);

        if (parameters.SearchTerm is not null)
        {
            var searchTerm = $"%{parameters.SearchTerm.ToLower()}%";

            query = query.Where(f =>
                f.Text != null && EF.Functions.Like(f.Text.ToLower(), searchTerm)
            );
        }

        return await query.CountAsync();
    }

    public async Task<Feedback?> GetByIdAsync(int id)
    {
        return await _dbContext.Feedbacks
            .FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task<decimal> GetAverageRatingAsync(int venueId) {
        return await _dbContext.Feedbacks
            .Where(f => f.VenueId == venueId)
            .AverageAsync(f => f.Rating);
    }

    public async Task<int> CreateAsync(Feedback feedback)
    {
        ArgumentNullException.ThrowIfNull(feedback);

        await _dbContext.Feedbacks.AddAsync(feedback);
        await _dbContext.SaveChangesAsync();

        return feedback.Id;
    }

    public async Task<int?> DeleteByIdAsync(int id)
    {
        var feedback = await _dbContext.Feedbacks.FindAsync(id);

        if (feedback is null)
            return null;
        
        _dbContext.Feedbacks.Remove(feedback);
        await _dbContext.SaveChangesAsync();

        return id;
    }

    public async Task<int?> PutAsync(Feedback feedback)
    {
        var feedbackToUpdate = await _dbContext.Feedbacks
            .FirstOrDefaultAsync(f => f.Id == feedback.Id);

        if (feedbackToUpdate == null)
            return null;

        feedbackToUpdate.Text = feedback.Text;
        feedbackToUpdate.Rating = feedback.Rating;
        feedbackToUpdate.CreationDate = feedback.CreationDate;
        feedbackToUpdate.UserId = feedback.UserId;

        await _dbContext.SaveChangesAsync();

        return feedback.Id;
    }

    public async Task<Feedback?> GetAsNoTrackingAsync(int id)
    {
        return await _dbContext.Feedbacks
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task<int?> IncrementLikesAsync(Feedback feedback)
    {
        var feedbackToUpdate = await _dbContext.Feedbacks
            .FirstOrDefaultAsync(mi => mi.Id == feedback.Id);

        if (feedbackToUpdate is null)
            return null;

        
        feedbackToUpdate.Likes++;

        await _dbContext.SaveChangesAsync();

        return feedback.Id;
    }

    public async Task<int?> DecrementLikesAsync(Feedback feedback)
    {
        var feedbackToUpdate = await _dbContext.Feedbacks
            .FirstOrDefaultAsync(mi => mi.Id == feedback.Id);

        if (feedbackToUpdate is null)
            return null;

        
        feedbackToUpdate.Likes--;

        await _dbContext.SaveChangesAsync();

        return feedback.Id;
    }
}
