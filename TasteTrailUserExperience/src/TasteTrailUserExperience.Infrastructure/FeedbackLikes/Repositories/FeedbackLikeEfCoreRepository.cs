using Microsoft.EntityFrameworkCore;
using TasteTrailUserExperience.Core.FeedbackLikes.Models;
using TasteTrailUserExperience.Core.FeedbackLikes.Repositories;
using TasteTrailUserExperience.Infrastructure.Common.Data;

namespace TasteTrailUserExperience.Infrastructure.FeedbackLikes.Repositories;

public class FeedbackLikeEfCoreRepository : IFeedbackLikeRepository
{
    private readonly UserExperienceDbContext _dbContext;

    public FeedbackLikeEfCoreRepository(UserExperienceDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }
    
    public async Task<int> CreateAsync(FeedbackLike feedbackLike)
    {
        ArgumentNullException.ThrowIfNull(feedbackLike);

        var feedback = await _dbContext.Feedbacks.FirstOrDefaultAsync(m => m.Id == feedbackLike.FeedbackId) ?? 
            throw new ArgumentException($"Feedback by ID: {feedbackLike.FeedbackId} not found.");

        await _dbContext.FeedbackLikes.AddAsync(feedbackLike);
        await _dbContext.SaveChangesAsync();

        return feedbackLike.Id;
    }

    public async Task<int?> DeleteByIdAsync(int id)
    {
        var feedbackLike = await _dbContext.FeedbackLikes.FindAsync(id);

        if (feedbackLike is null)
            return null;
        
        _dbContext.FeedbackLikes.Remove(feedbackLike);
        await _dbContext.SaveChangesAsync();

        return id;
    }

    public async Task<FeedbackLike?> GetAsNoTrackingAsync(int id)
    {
        return await _dbContext.FeedbackLikes
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task<List<int>> GetLikedFeedbacksIds(string userId)
    {
        return await _dbContext.FeedbackLikes
                             .Where(fl => fl.UserId == userId)
                             .Select(fl => fl.FeedbackId)
                             .ToListAsync();
    }

    public async Task<bool> Exists(int feedbackId, string userId)
    {
        return await _dbContext.FeedbackLikes
                             .AnyAsync(fl => fl.UserId == userId && fl.FeedbackId == feedbackId);
    }
}
